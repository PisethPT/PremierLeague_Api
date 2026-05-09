using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableRepository repository;
        private readonly ICacheService cacheService;

        public TableController(ITableRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        [HttpGet("get-tables")]
        public async Task<IActionResult> GetTables()
        {
            const int seasonId = 4;

            string cacheKey = $"table:season:{seasonId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetTableAsync(seasonId), 5);

            if (response is null || !response.IsSuccess)
            {
                return StatusCode(response?.StatusCode ?? 500, response);
            }

            return Ok(response);
        }
    }
}