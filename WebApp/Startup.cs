using Microsoft.Extensions.Options;
using NLog;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApp
{
    public class Startup
    {
        public IConfiguration Config { get; set; }

        public Startup(IConfiguration configuration)
        {
            Config = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddSingleton(x => initLoggerFactory());

            // skip custom services

            //services.AddProxies();

            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            //app.UseAuthentication(); // target
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static ILoggerFactory initLoggerFactory()
        {
            string logConfigPath = "nlog.config";
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(logConfigPath);

            return new NLogLoggerFactory();
        }
    }
}
