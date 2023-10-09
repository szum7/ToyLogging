using Microsoft.Extensions.Options;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NLog;
using NLog.Web;

namespace WebApiNLog
{
    public class ProgramOG
    {
        public static void MainOG(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // NLog: Setup NLog for Dependency injection
            //builder.Logging.ClearProviders();
            //builder.Host.UseNLog();

            builder.Services.AddSingleton(serviceProvider => initLoggerFactory(serviceProvider));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static ILoggerFactory initLoggerFactory(IServiceProvider provider)
        {
            var config = LogManager.LoadConfiguration(@"D:\CodeProjects\ToyLogging\WebApiNLog\nlog.config");

            if (LogManager.Configuration.FindTargetByName("database") is DatabaseTarget target)
            {

            }

            return new NLogLoggerFactory();
        }
    }

}
