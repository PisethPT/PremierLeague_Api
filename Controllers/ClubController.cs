using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly IClubRepository repository;

        public ClubController(IClubRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("get-season-clubs")]
        public async Task<IActionResult> GetSeasonClubs([FromQuery] int? season = null)
        {
            var response = await repository.GetSeasonClubsAsync(season);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-alltime-premierleague-clubs")]
        public async Task<IActionResult> GetAllTimePremierLeagueClubs()
        {
            var response = await repository.GetAllTimePremierLeagueClubsAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-club-detail")]
        public async Task<IActionResult> GetClubDetail([FromQuery] int clubId)
        {
            var response = await repository.GetClubDetailAsync(clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-club-overviews-nextmatch-teamform")]
        public async Task<IActionResult> GetNextMatchAndTeamForm([FromQuery] int clubId)
        {
            var response = await repository.GetClubOverviewNextMatchAndTeamFormAsync(clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-club-overviews-laststarting11")]
        public async Task<IActionResult> GetLastStarting11([FromQuery] int clubId)
        {
            var response = await repository.GetClubLastStarting11Async(clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        
        [HttpGet("get-club-newsandservices")]
        public async Task<IActionResult> GetClubNewsAndServices([FromQuery] int clubId)
        {
            var response = await repository.GetClubNewsAndServicesAsync(clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-club-table")]
        public async Task<IActionResult> GetClubTable([FromQuery] int seasonId, [FromQuery] int clubId)
        {
            var response = await repository.GetClubTableAsync(seasonId, clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-club-news")]
        public async Task<IActionResult> GetClubNews([FromQuery] int seasonId, [FromQuery] int clubId)
        {
            var response = await repository.GetClubNewsAsync(seasonId, clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-club-videos")]
        public async Task<IActionResult> GetClubVideos([FromQuery] int seasonId, [FromQuery] int clubId)
        {
            var response = await repository.GetClubVideosAsync(seasonId, clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-club-matches")]
        public async Task<IActionResult> GetClubMatches([FromQuery] int clubId, [FromQuery] int month)
        {
            var response = await repository.GetClubMatchesAsync(clubId: clubId, month: month);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }


        [HttpGet("get-club-squad")]
        public async Task<IActionResult> GetClubSquad([FromQuery] int clubId)
        {
            var response = await repository.GetClubSquadAsync(clubId:clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }


        [HttpGet("get-formations")]
        public async Task<IActionResult> GetFormations()
        {
            var response = await repository.GetFormationAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-club-monthofmatch")]
        public async Task<IActionResult> GetMonthOfMatch([FromQuery] int? seasonId, [FromQuery] int clubId)
        {
            var response = await repository.GetClubMonthOfMatchAsync(seasonId, clubId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
