using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Query;
using PremierLeague_Api.Repositories.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsRepository repository;

        public NewsController(INewsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("get-news-all")]
        public async Task<IActionResult> GetAllNews([FromQuery] NewsQuery query)
        {
            var response = await repository.GetNewsAsync(query);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
