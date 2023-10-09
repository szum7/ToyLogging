using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Targets;
using System;
using WebApiNLog.Models;

namespace WebApiNLog.Controllers
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
            // Needs the @ sign otherwise it writes "WebApiNLog.Models.Customer".

            var customer = new Customer
            {
                FirstName = $"Steve",
                LastName = $"Baker",
                DOB = new DateTime(1988, 8, 8),
                SSN = "123456789",
                PAN = $"45380000{DateTime.Now.Ticks.ToString().Substring(10, 8)}",
            };

            var FirstName = "James";
            var LastName = "Walker";
            var url = "?PhoneNo=12345678&IdentityNo=A33123123";

            //_logger.LogWarning("This is my customer {@Target}. End warning.", customer);
            //_logger.LogWarning("A single {@FirstName} property", FirstName);
            //_logger.LogWarning("A multiple {@FirstName} properties {@LastName}", FirstName, LastName);

            _logger.LogWarning("This is an url {url}.", url);

            return Ok("Ok.");
        }
    }
}