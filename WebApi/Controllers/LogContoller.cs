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
            var officeType = "remote";

            var customer = new Customer
            {
                FirstName = $"Steve01",
                LastName = $"Jobs01",
                Email = "myemail@gmail.com",
                DOB = new DateTime(1988, 8, 8),
                SSN = "123456789",
                PAN = $"45380000{DateTime.Now.Ticks.ToString().Substring(10, 8)}",
            };

            var office = new Office
            {
                Type = "remote",
                Number = 123123
            };

            _logger.LogWarning("Sensitive message with {Email} data.", Email);
            _logger.LogWarning("Not sensitive data with {OfficeType} data.", officeType);
            _logger.LogWarning("Sensitive object with {@Target} data.", customer);
            _logger.LogWarning("Not sensitive object with {@Target} data.", office);

            return Ok("Ok.");
        }
    }
}