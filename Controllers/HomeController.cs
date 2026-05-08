using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository repository;

        public HomeController(IHomeRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("get-home-club-news")]
        public async Task<IActionResult> GetClubNews()
        {
            var response = await repository.GetHomeClubNewsAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-home-matches")]
        public async Task<IActionResult> GetMatches()
        {
            var response = await repository.GetHomeMatchesAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-home-news")]
        public async Task<IActionResult> GetNews()
        {
            var response = await repository.GetHomeNewsAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-home-stories-news")]
        public async Task<IActionResult> GetStoriesNews([FromBody] List<string> videosTag, [FromQuery] int pageSize = 10)
        {
            var response = await repository.GetHomeStoriesNewsAsync(videosTag, pageSize);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-home-videos")]
        public async Task<IActionResult> GetVideos([FromBody] List<string> videoCategories)
        {
            if (videoCategories == null || !videoCategories.Any())
            {
                return BadRequest("Category list cannot be empty.");
            }

            var response = await repository.GetHomeVideosAsync(videoCategories);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-home-news")]
        public async Task<IActionResult> GetNews([FromBody] List<string> newsTags, [FromQuery] int pageSize = 5)
        {
            if (newsTags == null || !newsTags.Any())
            {
                return BadRequest("Category list cannot be empty.");
            }

            var response = await repository.GetHomeNewsAsync(newsTags, pageSize);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-home-news-from-the-clubs")]
        public async Task<IActionResult> GetFromTheClubs()
        {
            var response = await repository.GetHomeNewsFromTheClubsAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-home-news-multi-topics")]
        public async Task<IActionResult> GetNewsMulitTopic([FromBody] List<string> newsTags, [FromQuery] int pageSize)
        {
            if (newsTags == null || !newsTags.Any())
            {
                return BadRequest("Category list cannot be empty.");
            }

            var response = await repository.GetHomeNewsMulitTopicAsync(newsTags, pageSize);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-home-news-as-topics")]
        public async Task<IActionResult> GetNewsAsTopic([FromBody] List<string> newsTags, [FromQuery] int pageSize)
        {
            if (newsTags == null || !newsTags.Any())
            {
                return BadRequest("Category list cannot be empty.");
            }

            var response = await repository.GetHomeNewsAsTopicAsync(newsTags, pageSize);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-home-premierleague-news")]
        public async Task<IActionResult> GetPremierLeagueNewsOnly([FromQuery] int pageSize)
        {
            var response = await repository.GetHomePremierLeagueNewOnlyAsync(pageSize);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-home-quizzes-news")]
        public async Task<IActionResult> GetQuizzesNewsOnly([FromQuery] int pageSize)
        {
            var response = await repository.GetHomeQuizzesNewOnlyAsync(pageSize);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-video-viewer")]
        public async Task<IActionResult> GetVideoViewer([FromQuery] int videoId)
        {
            var response = await repository.GetHomeVideoViewerAsync(videoId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-news-viewer")]
        public async Task<IActionResult> GetNewsViewer([FromQuery] int newsId)
        {
            var response = await repository.GetHomeNewsViewerAsync(newsId);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-home-news-premierleaguegame")]
        public async Task<IActionResult> GetPremierLeagueGameNews()
        {
            var response = await repository.GetHomePermierLeagueGameNewsAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
