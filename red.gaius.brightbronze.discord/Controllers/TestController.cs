using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using red.gaius.brightbronze.discord.Models;

namespace red.gaius.brightbronze.discord.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Test Get()
        {
          AssemblyName assembly = Assembly.GetExecutingAssembly().GetName();
          return new Test(){
            AppName = assembly.Name,
            AppVersion = assembly.Version.ToString(),
            DateTimeNow = DateTime.Now.ToString(),
            RandomGuid = Guid.NewGuid().ToString()
          };
        }
    }
}
