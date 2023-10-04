using Microsoft.AspNetCore.Mvc;
using NLog;
using Slin.Masking.NLog;

namespace WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ILogger<LogController> _logger;

        public LogController(ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GenerateLog")]
        public IActionResult Get()
        {
            var valFromCode = "myemail@gmail.com";

            var logger = LogManager
                .Setup(setupBuilder: (setupBuilder) => setupBuilder.UseMasking("masking.json"))
                .GetCurrentClassLogger();

            logger.Warn("This is a warning message with param {param}", valFromCode);
            logger.Warn("This is an email myname@gmail.com");
            logger.Error(new Exception("My exception message."), "This is an error message with param {param}", valFromCode);

            //_logger.LogDebug("This is a debug message with param {param}", valFromCode);
            //_logger.LogInformation("This is an info message with param {param}", valFromCode);
            //_logger.LogWarning("This is a warning message with param {param}", valFromCode);
            //_logger.LogError(new Exception("My exception message."), "This is an error message with param {param}", valFromCode);

            return Ok("Ok.");
        }
    }
}