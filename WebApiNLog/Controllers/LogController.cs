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

        public LogController(
            ILogger<LogController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var PhoneNo = "23263476";
            var IdentityNo = "112211221122";

            _logger.LogWarning("This is my customer {PhoneNo} and {IdentityNo}.", PhoneNo, IdentityNo);

            return Ok("Ok.");
        }
    }
}