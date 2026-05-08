using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Query;
using PremierLeague_Api.Repositories.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerRepository repository;

        public PlayerController(IPlayerRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("get-players")]
        public async Task<IActionResult> GetPlayers([FromQuery] PlayerQuery? query)
        {
            var response = await repository.GetPlayersAsync(query);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-player-club")]
        public async Task<IActionResult> GetPlayerClub([FromQuery] int playerId)
        {
            var response = await repository.GetPlayerClubAsync(playerId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-player-information")]
        public async Task<IActionResult> GetPlayerInfo([FromQuery] int playerId)
        {
            var response = await repository.GetPlayerInfoAsync(playerId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-player-teammates")]
        public async Task<IActionResult> GetPlayerTeammates([FromQuery] int playerId)
        {
            var response = await repository.GetPlayerTeammatesAsync(playerId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
