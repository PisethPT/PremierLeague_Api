using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Repositories.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableRepository repository;

        public TableController(ITableRepository repository)
        {
            this.repository = repository;
        }
        [HttpGet("get-tables")]
        public async Task<IActionResult> GetTables()
        {
            var response = await repository.GetTableAsync(seasonId: 4);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
