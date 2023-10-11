namespace WebApi
{
    public class CustomLogger<T> : ICustomLogger<T>
    {
        private readonly ILogger<T> _logger;

        public CustomLogger(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogWarning(string? message, params object?[] args)
        {
            message = maskSensitiveInformation(message);

            _logger.LogWarning(message, args);
        }

        private string maskSensitiveInformation(string? message)
        {
            return message;
        }
    }
}
