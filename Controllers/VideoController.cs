using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository repository;
        private readonly ICacheService cacheService;

        public VideoController(IVideoRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        [HttpGet("get-video-latestvideos")]
        public async Task<IActionResult> GetLatestVideos()
        {
            const string cacheKey = "video:latest";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetLatestVideosAsync(), 5);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-video-series")]
        public async Task<IActionResult> GetSeries()
        {
            const string cacheKey = "video:series";

            var response =
                await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetSeriesAsync(), 60);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-video-thearchive")]
        public async Task<IActionResult> GetTheArchive()
        {
            const string cacheKey = "video:archive";

            var response =
                await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetTheArchiveAsync(), 1440);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-video-all")]
        public async Task<IActionResult> GetAllVideos([FromQuery] int page = 1)
        {
            string cacheKey = $"video:all:page:{page}";

            var response =
                await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetTheAllVideosAsync(page), 3);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }

        [HttpGet("get-video-all-count")]
        public async Task<IActionResult> GetAllVideosCount()
        {
            const string cacheKey = "video:count";

            var response = await cacheService.GetOrSetAsync(cacheKey, async () => await repository.GetTheAllVideosCountAsync(), 60);

            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            return Ok(response);
        }
    }
}