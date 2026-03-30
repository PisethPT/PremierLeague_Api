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
    }
}
