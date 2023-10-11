using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomLogController : ControllerBase
    {
        private readonly ICustomLogger<CustomLogController> _logger;

        public CustomLogController(ICustomLogger<CustomLogController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GenerateLogCustom")]
        public IActionResult Get()
        {
            var Email = "myemail@gmail.com";
            var officeType = "remote";

            _logger.LogWarning("Sensitive message with {Email} data.", Email);
            _logger.LogWarning("Not sensitive data with {OfficeType} data.", officeType);

            return Ok("Ok.");
        }
    }
}