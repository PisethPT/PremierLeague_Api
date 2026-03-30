using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace PremierLeague_Api.Controllers
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
        private readonly IDistributedCache _cache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

		[HttpGet(Name = "GetWeatherForecast")]
		public async Task<IEnumerable<WeatherForecast>> Get()
		{
			const string cacheKey = "weatherForecast";

			var cachedData = await _cache.GetStringAsync(cacheKey);
			if (!string.IsNullOrEmpty(cachedData))
			{
				return JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(cachedData)!;
			}

			var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
			{
				Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
				TemperatureC = Random.Shared.Next(-20, 55),
				Summary = Summaries[Random.Shared.Next(Summaries.Length)]
			}).ToArray();

			await _cache.SetStringAsync(
				cacheKey,
				JsonSerializer.Serialize(data),
				new DistributedCacheEntryOptions
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
				});

			return data;
		}
	}
}
