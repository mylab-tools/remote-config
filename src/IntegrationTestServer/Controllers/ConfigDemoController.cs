using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace IntegrationTestServer.Controllers
{
    [Route("api/config")]
    [ApiController]
    public class ConfigDemoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private static readonly string[] ConfigKeys = 
        {
            "FooParam",
            "ParamForOverride",
            "SecretParam",
            "InnerObject",
            "Redis",
            "Rabbit"
        };

        public ConfigDemoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var dict = new Dictionary<string, string>();

            foreach (var configKey in _configuration.AsEnumerable()
                .Where(k => ConfigKeys.Any(ck => k.Key.StartsWith(ck))))
                dict.Add(configKey.Key, configKey.Value);

            return Ok(dict);
        }
    }
}
