﻿using NLog;
using NLog.Web;
using Slin.Masking;
using Slin.Masking.NLog;

namespace WebApp.OG
{
    public class ProgramOG
    {
        public static void MainOG(string[] args)
        {
            // Early init of NLog to allow startup and exception logging, before host is built
            //var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

            var logger = LogManager
                .Setup(setupBuilder: (setupBuilder) => setupBuilder.UseMasking("masking.json"))
                .GetCurrentClassLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                // NLog: Setup NLog for Dependency injection
                builder.Logging.ClearProviders();
                builder.Host.UseNLog();


                var app = builder.Build();

                //builder.Services
                //    .Configure<MaskingProfile>(app.Configuration.GetSection("Masking"))
                //    .PostConfigure<MaskingProfile>(options => options.Normalize());

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
            catch (Exception exception)
            {
                // NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }
    }

}
