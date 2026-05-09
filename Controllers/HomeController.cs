using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeRepository repository;
        private readonly ICacheService cacheService;

        public HomeController(IHomeRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        [HttpGet("get-home-club-news")]
        public async Task<IActionResult> GetClubNews()
        {
            const string cacheKey = "home:clubnews";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeClubNewsAsync(), 5);

            return HandleResponse(response);
        }

        [HttpGet("get-home-matches")]
        public async Task<IActionResult> GetMatches()
        {
            const string cacheKey = "home:matches";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeMatchesAsync(), 1);

            return HandleResponse(response);
        }

        [HttpGet("get-home-news")]
        public async Task<IActionResult> GetNews()
        {
            const string cacheKey = "home:news";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeNewsAsync(), 5);

            return HandleResponse(response);
        }

        [HttpPost("get-home-stories-news")]
        public async Task<IActionResult> GetStoriesNews([FromBody] List<string> videosTag, [FromQuery] int pageSize = 10)
        {
            if (videosTag == null || !videosTag.Any())
            {
                return BadRequest("Tag list cannot be empty.");
            }

            videosTag = videosTag
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            string cacheKey = $"home:stories:{string.Join("-", videosTag)}:{pageSize}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeStoriesNewsAsync(videosTag, pageSize), 5);

            return HandleResponse(response);
        }

        [HttpPost("get-home-videos")]
        public async Task<IActionResult> GetVideos([FromBody] List<string> videoCategories)
        {
            if (videoCategories == null || !videoCategories.Any())
            {
                return BadRequest("Category list cannot be empty.");
            }

            videoCategories = videoCategories
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            string cacheKey = $"home:videos:{string.Join("-", videoCategories)}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeVideosAsync(videoCategories), 10);

            return HandleResponse(response);
        }

        [HttpPost("get-home-news")]
        public async Task<IActionResult> GetNews([FromBody] List<string> newsTags, [FromQuery] int pageSize = 5)
        {
            if (newsTags == null || !newsTags.Any())
            {
                return BadRequest("Category list cannot be empty.");
            }

            newsTags = newsTags
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            string cacheKey = $"home:news:{string.Join("-", newsTags)}:{pageSize}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeNewsAsync(newsTags, pageSize), 5);

            return HandleResponse(response);
        }

        [HttpGet("get-home-news-from-the-clubs")]
        public async Task<IActionResult> GetFromTheClubs()
        {
            const string cacheKey = "home:clubsnews";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeNewsFromTheClubsAsync(), 5);

            return HandleResponse(response);
        }

        [HttpPost("get-home-news-multi-topics")]
        public async Task<IActionResult> GetNewsMulitTopic([FromBody] List<string> newsTags, [FromQuery] int pageSize)
        {
            if (newsTags == null || !newsTags.Any())
            {
                return BadRequest("Category list cannot be empty.");
            }

            newsTags = newsTags
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            string cacheKey = $"home:newsmulti:{string.Join("-", newsTags)}:{pageSize}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeNewsMulitTopicAsync(newsTags, pageSize), 5);

            return HandleResponse(response);
        }

        [HttpPost("get-home-news-as-topics")]
        public async Task<IActionResult> GetNewsAsTopic([FromBody] List<string> newsTags, [FromQuery] int pageSize)
        {
            if (newsTags == null || !newsTags.Any())
            {
                return BadRequest("Category list cannot be empty.");
            }

            newsTags = newsTags
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            string cacheKey = $"home:newstopics:{string.Join("-", newsTags)}:{pageSize}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeNewsAsTopicAsync(newsTags, pageSize), 5);

            return HandleResponse(response);
        }

        [HttpPost("get-home-premierleague-news")]
        public async Task<IActionResult> GetPremierLeagueNewsOnly(
            [FromQuery] int pageSize)
        {
            string cacheKey = $"home:premierleague:{pageSize}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomePremierLeagueNewOnlyAsync(pageSize), 5);

            return HandleResponse(response);
        }

        [HttpPost("get-home-quizzes-news")]
        public async Task<IActionResult> GetQuizzesNewsOnly([FromQuery] int pageSize)
        {
            string cacheKey = $"home:quizzes:{pageSize}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeQuizzesNewOnlyAsync(pageSize), 10);

            return HandleResponse(response);
        }

        [HttpGet("get-video-viewer")]
        public async Task<IActionResult> GetVideoViewer(
            [FromQuery] int videoId)
        {
            string cacheKey = $"video:viewer:{videoId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeVideoViewerAsync(videoId), 60);

            return HandleResponse(response);
        }

        [HttpGet("get-news-viewer")]
        public async Task<IActionResult> GetNewsViewer([FromQuery] int newsId)
        {
            string cacheKey = $"news:viewer:{newsId}";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomeNewsViewerAsync(newsId), 60);

            return HandleResponse(response);
        }

        [HttpGet("get-home-news-premierleaguegame")]
        public async Task<IActionResult> GetPremierLeagueGameNews()
        {
            const string cacheKey = "home:premierleaguegame";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetHomePermierLeagueGameNewsAsync(), 5);

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