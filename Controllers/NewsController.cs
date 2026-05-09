using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Query;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository repository;
        private readonly ICacheService cacheService;

        public NewsController(INewsRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        [HttpGet("get-news-all")]
        public async Task<IActionResult> GetAllNews([FromQuery] NewsQuery query)
        {
            string cacheKey = GenerateCacheKey(query);

            var response =
                await cacheService.GetOrSetAsync(
                    cacheKey, async () => await repository.GetNewsAsync(query), 5);

            if (response is null || !response.IsSuccess)
            {
                return StatusCode(response?.StatusCode ?? 500, response);
            }

            return Ok(response);
        }

        private static string GenerateCacheKey(NewsQuery query)
        {
            return string.Join(":",
                "news",
                $"category-{query.NewsCategoryId?.ToString() ?? "all"}",
                $"page-{query.PageNumber}",
                $"size-{query.PageSize}"
            ).ToLower();
        }
    }
}