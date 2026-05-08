using Azure.Core;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Query;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        private readonly IJwtService jwtService;

        public AuthController(IAuthRepository repository, IJwtService jwtService)
        {
            this.repository = repository;
            this.jwtService = jwtService;
        }

        [HttpPost("signin-google")]
        public async Task<IActionResult> SigninGoogle([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential);

                var siginGoogleDto = new SiginGoogleDto
                {
                    Email = payload.Email,
                    GoogleId = payload.Subject,
                    FirstName = payload.GivenName ?? "Guest",
                    LastName = payload.FamilyName ?? "User",
                    Locale = payload.Locale,
                    PhotoUrl = payload.Picture
                };

                var userCheck = await repository.LoginAccountAlreadyAsync(siginGoogleDto.Email);

                SiginGoogleDto userInDb;

                if (!userCheck.Contents)
                {
                    var createResponse = await repository.SiginGoogleAccontAsync(siginGoogleDto);
                    if (!createResponse.IsSuccess) return BadRequest(createResponse);

                    var freshUser = await repository.GetUserByEmailAsync(siginGoogleDto.Email);
                    userInDb = freshUser.Contents;
                }
                else
                {
                    var existingUser = await repository.GetUserByEmailAsync(siginGoogleDto.Email);
                    userInDb = existingUser.Contents;
                }

                var accessToken = jwtService.GenerateToken(userInDb);
                var refreshToken = jwtService.GenerateRefreshToken();

                await repository.SaveRefreshTokenAsync(userInDb.UserId!, refreshToken);

                var authResponse = new AuthResponseDto
                {
                    Email = payload.Email,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    ExpiredAt = DateTime.UtcNow.AddMinutes(30)
                };

                return Ok(new Response<AuthResponseDto>(200, "Sign in success", authResponse, true));
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(new Response<string>(401, "Invalid Google Credentials", null!, false));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<string>(500, ex.Message, null!, false));
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var tokenInDb = await repository.GetRefreshTokenAsync(request.RefreshToken);

            if (!tokenInDb.IsSuccess || tokenInDb.Contents == null)
                return Unauthorized(new Response<string>(401, "Invalid Session", null!, false));

            var response = await repository.GetUserByIdAsync(tokenInDb.Contents.UserId);

            var newAccessToken = jwtService.GenerateToken(response.Contents);
            var newRefreshToken = jwtService.GenerateRefreshToken();

            await repository.SaveRefreshTokenAsync(response.Contents.UserId!, newRefreshToken);

            return Ok(new Response<AuthResponseDto>(200, "Token Refreshed", new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiredAt = DateTime.UtcNow.AddMinutes(30)
            }, true));
        }

        [HttpPost("get-fav-clubs")]
        public async Task<IActionResult> GetFavClubs([FromBody] string? email)
        {
            var response = await repository.GetFavClubsAsync(email);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-fav-selectedclub")]
        public async Task<IActionResult> GetFavSeletedClubs([FromBody] string? email)
        {
            var response = await repository.GetFavSelectedClubsAsync(email);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-fav-players")]
        public async Task<IActionResult> GetFavPlayers([FromBody] FavPlayerRequest? request)
        {
            var response = await repository.GetFavPlayersAsync(request);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("fav-saveselectedclubs")]
        public async Task<IActionResult> SaveSelectedClubs([FromBody] FavRequest? request)
        {
            var response = await repository.SaveSelectedClubsAsync(request);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("fav-saveselectedplayers")]
        public async Task<IActionResult> SaveSelectedPlayers([FromBody] FavRequest? request)
        {
            var response = await repository.SaveSelectedPlayersAsync(request);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("fav-checkuserfavorite")]
        public async Task<IActionResult> CheckUserFavorite([FromBody] string? email)
        {
            var response = await repository.CheckUserFavoritesAsync(email);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }

        [HttpPost("get-fav-myPLSettings")]
        public async Task<IActionResult>GetmyPLSettings([FromBody] string? email)
        {
            var response = await repository.GetmyPLSettingsAsync(email);
            if (response is null || !response.IsSuccess)
                return StatusCode(response?.StatusCode ?? 500, response);
            return Ok(response);
        }
    }
}
