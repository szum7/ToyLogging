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
            _logger.LogInformation("My information log {timeTicks}", DateTime.Now.Ticks);

            return Ok();
        }
    }
}