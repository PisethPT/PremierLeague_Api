using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchRepository repository;

        public MatchController(IMatchRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("get-matches")]
        public async Task<IActionResult> GetMatches([FromQuery] int matchWeek, CancellationToken ct = default)
        {
            var response = await repository.GetMatchesAsync(matchWeek, ct);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-matches-info-detail")]
        public async Task<IActionResult> GetMatchInfoDetail([FromQuery] int matchId)
        {
            var response = await repository.GetMatchInfoDetailAsync(matchId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-matches-story")]
        public async Task<IActionResult> GetMatchStory([FromQuery] int matchId)
        {
            var response = await repository.GetMatchStoryAsync(matchId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-matches-recap")]
        public async Task<IActionResult> GetMatchRecap([FromQuery] int matchId)
        {
            var response = await repository.GetMatchRecapAsync(matchId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-matches-relatedcontent")]
        public async Task<IActionResult> GetMatchRelatedContent([FromQuery] int matchId)
        {
            var response = await repository.GetMatchRelatedContentAsync(matchId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }


        [HttpGet("get-matches-highlight")]
        public async Task<IActionResult> GetMatchHighlight([FromQuery] int matchId)
        {
            var response = await repository.GetMatchHighlightAsync(matchId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-matches-lineup")]
        public async Task<IActionResult> GetMatchLineup([FromQuery] int matchId)
        {
            var response = await repository.GetMatchLinupAsync(matchId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-matches-matchinfo")]
        public async Task<IActionResult> GetMatchInfo([FromQuery] int matchId)
        {
            var response = await repository.GetMatchInfoAsync(matchId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
