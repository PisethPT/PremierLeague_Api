using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly IClubRepository repository;
        private readonly ICacheService cacheService;

        public ClubController(IClubRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        [HttpGet("get-season-clubs")]
        public async Task<IActionResult> GetSeasonClubs([FromQuery] int? season = null)
        {
            string cacheKey = $"club:season:{season ?? 0}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetSeasonClubsAsync(season), 10);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-alltime-premierleague-clubs")]
        public async Task<IActionResult> GetAllTimePremierLeagueClubs()
        {
            const string cacheKey = "club:alltime";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetAllTimePremierLeagueClubsAsync(), 1440); // 24h

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-detail")]
        public async Task<IActionResult> GetClubDetail([FromQuery] int clubId)
        {
            string cacheKey = $"club:detail:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubDetailAsync(clubId), 30);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-overviews-nextmatch-teamform")]
        public async Task<IActionResult> GetNextMatchAndTeamForm([FromQuery] int clubId)
        {
            string cacheKey = $"club:overview:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubOverviewNextMatchAndTeamFormAsync(clubId), 2);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-overviews-laststarting11")]
        public async Task<IActionResult> GetLastStarting11([FromQuery] int clubId)
        {
            string cacheKey = $"club:last11:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubLastStarting11Async(clubId), 2);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-newsandservices")]
        public async Task<IActionResult> GetClubNewsAndServices([FromQuery] int clubId)
        {
            string cacheKey = $"club:newsservices:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubNewsAndServicesAsync(clubId), 10);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-table")]
        public async Task<IActionResult> GetClubTable([FromQuery] int seasonId, [FromQuery] int clubId)
        {
            string cacheKey = $"club:table:{seasonId}:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubTableAsync(seasonId, clubId), 5);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-news")]
        public async Task<IActionResult> GetClubNews([FromQuery] int seasonId, [FromQuery] int clubId)
        {
            string cacheKey = $"club:news:{seasonId}:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubNewsAsync(seasonId, clubId), 5);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-videos")]
        public async Task<IActionResult> GetClubVideos([FromQuery] int seasonId, [FromQuery] int clubId)
        {
            string cacheKey = $"club:videos:{seasonId}:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubVideosAsync(seasonId, clubId), 10);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-matches")]
        public async Task<IActionResult> GetClubMatches([FromQuery] int clubId, [FromQuery] int month)
        {
            string cacheKey = $"club:matches:{clubId}:{month}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubMatchesAsync(clubId, month), 2);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-squad")]
        public async Task<IActionResult> GetClubSquad([FromQuery] int clubId)
        {
            string cacheKey = $"club:squad:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubSquadAsync(clubId), 60);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-formations")]
        public async Task<IActionResult> GetFormations()
        {
            const string cacheKey = "club:formations";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetFormationAsync(), 1440);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-club-monthofmatch")]
        public async Task<IActionResult> GetMonthOfMatch([FromQuery] int? seasonId, [FromQuery] int clubId)
        {
            string cacheKey = $"club:months:{seasonId ?? 0}:{clubId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetClubMonthOfMatchAsync(seasonId, clubId), 10);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }
    }
}