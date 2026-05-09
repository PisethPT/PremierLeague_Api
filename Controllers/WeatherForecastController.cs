using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace PremierLeague_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool",
            "Mild", "Warm", "Balmy", "Hot",
            "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDistributedCache _cache;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            const string cacheKey = "weatherForecast";

            try
            {
                var cachedData = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrWhiteSpace(cachedData))
                {
                    _logger.LogInformation("Data loaded from Redis cache.");

                    var cachedResult =
                        JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(
                            cachedData,
                            _jsonOptions);

                    return Ok(cachedResult);
                }

                var data = Enumerable.Range(1, 5)
                    .Select(index => new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    })
                    .ToArray();

 
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
                    SlidingExpiration = TimeSpan.FromSeconds(30)
                };

                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(data, _jsonOptions),
                    cacheOptions);

                _logger.LogInformation("Data saved to Redis cache.");

                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Redis cache error.");

                return StatusCode(500, "Internal server error");
            }
        }
    }
}