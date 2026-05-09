using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Dtos.SelectListItemDto;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/premierleague/[controller]")]
    [ApiController]
    public class SelectListItemController : ControllerBase
    {
        private readonly ISelectListItemRepository repository;
        private readonly ICacheService cacheService;

        public SelectListItemController(ISelectListItemRepository repository, ICacheService cacheService)
        {
            this.repository = repository;
            this.cacheService = cacheService;
        }

        [HttpGet("get-selectlistitem-clubs")]
        public async Task<IActionResult> GetSelectListItemClubs()
        {
            const string cacheKey = "selectlist:clubs";

            var response = await cacheService.GetOrSetAsync(cacheKey, 
                    async () => await repository.SelectListItemAsync<SelectListItemClubDto>(
                            "PL_ApiSelectListItemClub",
                            rdr => new SelectListItemClubDto
                            {
                                ClubId = rdr.SafeGetInt("ClubId"),
                                ClubName = rdr.SafeGetString("ClubName"),
                                ClubCrest = rdr.SafeGetString("ClubCrest"),
                                ClubTheme = rdr.SafeGetString("ClubTheme"),
                            }),
                    1440); // 24 hours

            if (response is null || !response.IsSuccess)
            {
                return StatusCode(response?.StatusCode ?? 500, response);
            }

            return Ok(response);
        }
    }
}