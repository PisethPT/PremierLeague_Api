using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly IVideoRepository repository;

        public VideoController(IVideoRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("get-video-latestvideos")]
        public async Task<IActionResult> GetLatestVideos()
        {
            var response = await repository.GetLatestVideosAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-video-series")]
        public async Task<IActionResult> GetSeries()
        {
            var response = await repository.GetSeriesAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-video-thearchive")]
        public async Task<IActionResult> GetTheArchive()
        {
            var response = await repository.GetTheArchiveAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-video-all")]
        public async Task<IActionResult> GetAllVideos([FromQuery] int page = 1)
        {
            var response = await repository.GetTheAllVideosAsync(page);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpGet("get-video-all-count")]
        public async Task<IActionResult> GetAllVideosCount()
        {
            var response = await repository.GetTheAllVideosCountAsync();
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
