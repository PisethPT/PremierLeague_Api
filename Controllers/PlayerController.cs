using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Query;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository repository;
        private readonly ICacheService cacheService;

        public PlayerController(IPlayerRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        [HttpGet("get-players")]
        public async Task<IActionResult> GetPlayers([FromQuery] PlayerQuery? query)
        {
            query ??= new PlayerQuery();

            NormalizeQuery(query);

            string cacheKey = GeneratePlayersCacheKey(query);

            var response =
                await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetPlayersAsync(query), 5);

            if (response is null || !response.IsSuccess)
            {
                return StatusCode(response?.StatusCode ?? 500, response);
            }

            return Ok(response);
        }

        [HttpGet("get-player-club")]
        public async Task<IActionResult> GetPlayerClub([FromQuery] int playerId)
        {
            string cacheKey = $"player:club:{playerId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetPlayerClubAsync(playerId), 30);

            if (response is null || !response.IsSuccess)
            {
                return StatusCode(response?.StatusCode ?? 500, response);
            }

            return Ok(response);
        }

        [HttpGet("get-player-information")]
        public async Task<IActionResult> GetPlayerInfo([FromQuery] int playerId)
        {
            string cacheKey = $"player:info:{playerId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetPlayerInfoAsync(playerId), 60);

            if (response is null || !response.IsSuccess)
            {
                return StatusCode(response?.StatusCode ?? 500, response);
            }

            return Ok(response);
        }

        [HttpGet("get-player-teammates")]
        public async Task<IActionResult> GetPlayerTeammates([FromQuery] int playerId)
        {
            string cacheKey = $"player:teammates:{playerId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetPlayerTeammatesAsync(playerId), 30);

            if (response is null || !response.IsSuccess)
            {
                return StatusCode(response?.StatusCode ?? 500, response);
            }

            return Ok(response);
        }

        #region HELPERS

        private static void NormalizeQuery(PlayerQuery query)
        {
            query.Page = query.Page <= 0 ? 1 : query.Page;

            query.PageSize = query.PageSize <= 0 || query.PageSize > 100 ? 20 : query.PageSize;

            query.Clubs = query.Clubs
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            query.Positions = query.Positions
                .Distinct()
                .OrderBy(x => x)
                .ToList();
        }

        private static string GeneratePlayersCacheKey(PlayerQuery query)
        {
            string clubs = query.Clubs.Any() ? string.Join("-", query.Clubs) : "all";

            string positions = query.Positions.Any() ? string.Join("-", query.Positions) : "all";

            return string.Join(":",
                "players",
                $"page-{query.Page}",
                $"size-{query.PageSize}",
                $"competition-{query.Competition?.ToString() ?? "all"}",
                $"season-{query.Season?.ToString() ?? "all"}",
                $"clubs-{clubs}",
                $"positions-{positions}"
            ).ToLower();
        }

        #endregion
    }
}