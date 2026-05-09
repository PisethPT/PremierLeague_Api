using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchRepository repository;
        private readonly ICacheService cacheService;

        public MatchController(IMatchRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        [HttpGet("get-matches")]
        public async Task<IActionResult> GetMatches([FromQuery] int matchWeek, CancellationToken ct = default)
        {
            string cacheKey = $"match:list:week:{matchWeek}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetMatchesAsync(matchWeek, ct), 2);

            return HandleResponse(response);
        }

        [HttpGet("get-matches-info-detail")]
        public async Task<IActionResult> GetMatchInfoDetail([FromQuery] int matchId)
        {
            string cacheKey = $"match:detail:{matchId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetMatchInfoDetailAsync(matchId), 5);

            return HandleResponse(response);
        }

        [HttpGet("get-matches-story")]
        public async Task<IActionResult> GetMatchStory([FromQuery] int matchId)
        {
            string cacheKey = $"match:story:{matchId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetMatchStoryAsync(matchId), 5);

            return HandleResponse(response);
        }

        [HttpGet("get-matches-recap")]
        public async Task<IActionResult> GetMatchRecap(
            [FromQuery] int matchId)
        {
            string cacheKey = $"match:recap:{matchId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetMatchRecapAsync(matchId), 10);

            return HandleResponse(response);
        }

        [HttpGet("get-matches-relatedcontent")]
        public async Task<IActionResult> GetMatchRelatedContent(
            [FromQuery] int matchId)
        {
            string cacheKey = $"match:related:{matchId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetMatchRelatedContentAsync(matchId), 10);

            return HandleResponse(response);
        }

        [HttpGet("get-matches-highlight")]
        public async Task<IActionResult> GetMatchHighlight([FromQuery] int matchId)
        {
            string cacheKey = $"match:highlight:{matchId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetMatchHighlightAsync(matchId), 10);

            return HandleResponse(response);
        }

        [HttpGet("get-matches-lineup")]
        public async Task<IActionResult> GetMatchLineup(
            [FromQuery] int matchId)
        {
            string cacheKey =
                $"match:lineup:{matchId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetMatchLinupAsync(matchId), 2);

            return HandleResponse(response);
        }

        [HttpGet("get-matches-matchinfo")]
        public async Task<IActionResult> GetMatchInfo([FromQuery] int matchId)
        {
            string cacheKey = $"match:info:{matchId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetMatchInfoAsync(matchId), 2);

            return HandleResponse(response);
        }

        #region PRIVATE METHODS

        private IActionResult HandleResponse(dynamic response)
        {
            if (response is null || !response.IsSuccess)
            {
                return StatusCode(response?.StatusCode ?? 500, response);
            }

            return Ok(response);
        }

        #endregion
    }
}