using Microsoft.AspNetCore.Mvc;

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
            var valFromCode = "ValueFromCode";

            _logger.LogDebug("This is a debug message with param {param}", valFromCode);
            _logger.LogInformation("This is an info message with param {param}", valFromCode);
            _logger.LogWarning("This is a warning message with param {param}", valFromCode);
            _logger.LogError(new Exception("My exception message."), "This is an error message with param {param}", valFromCode);

            return Ok("Ok.");
        }
    }
}