using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Repositories.Implementations;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class AiController : ControllerBase
    {
        private readonly IAiRepository aiRepository;
        private readonly IAiQueryService aiQuery;

        public AiController(IAiRepository aiRepository, IAiQueryService aiQuery)
        {
            this.aiRepository = aiRepository;
            this.aiQuery = aiQuery;
        }


        [HttpPost("ask")]
        public async Task<IActionResult> AskAIAgent([FromBody] string userQuestion)
        {
            var content = await aiQuery.GenerateSqlAsync(userQuestion);
            if (string.IsNullOrEmpty(content.Sql))
                return BadRequest("");

            var response = await aiRepository.AiQuery(content.Sql);
  
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);

            var results = new AiAssistantDto
            {
                Data = response.Contents,
                Chat = new AiResponseDto
                {
                    Title = content.Title,
                    Description = content.Description,
                    Tip = content.Tip,
                    Sql = content.Sql,
                },
            };

            return Ok(results);
        }
    }
}
