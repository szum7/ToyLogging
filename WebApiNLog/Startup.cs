using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Filters;
using NLog.Targets;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Numerics;
using WebApiNLog.CustomRender;

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

            //services.AddSingleton(serviceProvider => initLoggerFactory(serviceProvider));

            services.RegisterNLogSensitiveDataMask(
                new NLogSensitiveDataMaskOptions
                {
                    MaskDataDic = new Dictionary<string, Func<string, string>>
                    {
                        { "PhoneNo", c => c?.ToString()?[..2] + "***" },
                        { "IdentityNo", c => c?.ToString()?[..2] + "***" }
                    }
                });

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton(Config);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static ILoggerFactory initLoggerFactory(IServiceProvider provider)
        {
            string logConfigPath = "nlog.config";
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(logConfigPath);

            if (LogManager.Configuration.FindTargetByName("database") is DatabaseTarget target)
            {
            }

            LogManager.Setup().SetupSerialization(s =>
                s.RegisterObjectTransformation<object>(o =>
                {
                    // TODO LogManager.Configuration.FindTargetByName("database")

                    var props = o.GetType().GetProperties();
                    var propsDict = props.ToDictionary(p => p.Name, p => p.GetValue(o));

                    if (propsDict.TryGetValue("FirstName", out var phone))
                        propsDict["FirstName"] = phone?.ToString()?[..2] + "***";

                    if (propsDict.TryGetValue("LastName", out var id))
                        propsDict["LastName"] = id?.ToString()?[..2] + "***";

                    return propsDict;
                }));

            return new NLogLoggerFactory();
        }
    }
}