﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Filters;
using NLog.Targets;
using Slin.Masking;
using Slin.Masking.NLog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi
{
    public class Startup
    {
        public IConfiguration Config { get; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddSingleton(serviceProvider => initLoggerFactory(serviceProvider));

            //services.AddProxies(); // target

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<CustomLogger>();
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ICustomLogger<>), typeof(CustomLogger<>)));

            services.AddSingleton(Config);

            services
                .AddControllers(opt =>
                {
                    opt.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                    opt.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                    opt.InputFormatters.Insert(0, getJsonPatchInputFormatter());
                })
                .AddNewtonsoftJson(n =>
                {
                    n.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    n.SerializerSettings.Converters.Add(new StringEnumConverter());
                });

            services
                .Configure<MaskingProfile>(Config.GetSection("Masking"))
                .PostConfigure<MaskingProfile>((option) => option.Normalize());
        }

        private static NewtonsoftJsonInputFormatter getJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonInputFormatter>()
                .First();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(x =>
            {
                x.AllowAnyOrigin();
                x.AllowAnyMethod();
                x.AllowAnyHeader();
            }); // target

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting(); // target

            //app.UseAuthentication(); // target
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static ILoggerFactory initLoggerFactory(IServiceProvider provider)
        {
            LogManager
                .Setup((setupBuilder) => setupBuilder
                .LoadConfigurationFromFile()
                .UseMasking());

            if (LogManager.Configuration.FindTargetByName("databaseSecure") is DatabaseTarget secureTarget)
            {
                var profile = provider.GetRequiredService<IOptions<MaskingProfile>>().Value;
                var profileRules = profile.Rules;

                var loggingRule = new LoggingRule("*", NLog.LogLevel.Info, secureTarget);

                foreach (var rule in profileRules)
                {
                    loggingRule.Filters.Add(new ConditionBasedFilter()
                    {
                        Condition = $"contains('${{message:raw=true}}', '{{{rule.Key}}}')",
                        Action = FilterResult.Log
                    });
                }

                LogManager.Configuration.LoggingRules.Add(loggingRule);
            }

            return new NLogLoggerFactory();
        }
    }
}
