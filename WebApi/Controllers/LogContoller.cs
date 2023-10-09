using Microsoft.AspNetCore.Mvc;
using NLog;
using Slin.Masking.NLog;
using WebApi.Models;

namespace WebApi.Controllers
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
            var Email = "myemail@gmail.com";
            var customer = new Customer
            {
                FirstName = $"Steve01",
                LastName = $"Jobs01",
                DOB = new DateTime(1988, 8, 8),
                SSN = "123456789",
                PAN = $"45380000{DateTime.Now.Ticks.ToString().Substring(10, 8)}",
            };

            var logger = LogManager
                .Setup(setupBuilder: (setupBuilder) => setupBuilder.UseMasking("masking.json"))
                .GetCurrentClassLogger();

            logger.Warn("Message: logger.Warn - myname@gmail.com, {Email}", Email);
            _logger.LogWarning("Message: _logger.LogWarning - myname@gmail.com, {Email}", Email);
            logger.Warn("---Structured {@Target}---", customer);
            _logger.LogWarning("---Structured {@Target}---", customer);

            return Ok("Ok.");
        }
    }
}