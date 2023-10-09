using NLog.Config;
using NLog;

namespace WebApiNLog.CustomRender
{
    public static class NLogExtensions
    {
        public static IServiceCollection RegisterNLogSensitiveDataMask(this IServiceCollection services, NLogSensitiveDataMaskOptions options)
        {
            ConfigurationItemFactory.Default.CreateInstance = type =>
            {
                if (type != typeof(SensitiveMaskLayoutRenderer))
                    return Activator.CreateInstance(type);

                return new SensitiveMaskLayoutRenderer(options);
            };

            LogManager.Setup()
                .SetupExtensions(s => s.RegisterLayoutRenderer<SensitiveMaskLayoutRenderer>("sensitive-mask"));

            LogManager.Configuration = LogManager.Configuration?.Reload();

            return services;
        }
    }
}
