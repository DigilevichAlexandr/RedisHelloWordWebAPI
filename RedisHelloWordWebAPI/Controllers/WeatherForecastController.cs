using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisHelloWordWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IDatabase _database;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IDatabase database)
        {
            _logger = logger;
            _database = database;
        }

        //[HttpGet]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = rng.Next(-20, 55),
        //        Summary = Summaries[rng.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        [HttpGet("{key}")]
        public async Task<string> GetAsync(string key)
        {
            return await _database.StringGetAsync(key);
        }

        [HttpPost]
        public async Task PostAsync([FromBody] KeyValuePair<string, string> keyValue)
        {
            await _database.StringSetAsync(keyValue.Key, keyValue.Value);
        }

        [HttpPost(template: "expiry")]
        public async Task SaveExpiry([FromBody] KeyValuePair<string, string> keyValue)
        {
            await _database.StringSetAsync(keyValue.Key, keyValue.Value, expiry: TimeSpan.FromSeconds(10));
        }
    }
}
