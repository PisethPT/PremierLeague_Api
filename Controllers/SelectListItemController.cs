using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Dtos.SelectListItemDto;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class SelectListItemController : ControllerBase
    {
        private readonly ISelectListItemRepository repository;

        public SelectListItemController(ISelectListItemRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("get-selectlistitem-clubs")]
        public async Task<IActionResult> GetSelectListItemClubs()
        {
            var response = await repository.SelectListItemAsync<SelectListItemClubDto>("PL_ApiSelectListItemClub",
                rdr => new SelectListItemClubDto
                {
                    ClubId = rdr.SafeGetInt("ClubId"),
                    ClubName = rdr.SafeGetString("ClubName"),
                    ClubCrest = rdr.SafeGetString("ClubCrest"),
                    ClubTheme = rdr.SafeGetString("ClubTheme"),
                });
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
