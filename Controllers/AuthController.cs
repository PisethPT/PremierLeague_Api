using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using PremierLeague_Api.Dtos;
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
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential);

            var email = payload.Email;
            var googleId = payload.Subject;
            var firstName = payload.GivenName ?? "Guest";
            var lastName = payload.FamilyName ?? "User";
            var photoUrl = payload.Picture;

            var siginGoogleDto = new SiginGoogleDto
            {
                Email = email,
                GoogleId = googleId,
                FirstName = firstName,
                LastName = lastName,
                PhotoUrl = photoUrl
            };

            var isExisting = await repository.OAuthLoginAccountAlreadyAsync(email);

            if (!isExisting.Contents)
            {
                await repository.OAuthSiginGoogleAccontAsync(siginGoogleDto);
            }
            else
            {
                 
            }

            var accessToken = jwtService.GenerateToken(siginGoogleDto);
            var refreshToken = jwtService.GenerateRefreshToken();
            await repository.SaveRefreshTokenAsync(siginGoogleDto.GooogleId!, refreshToken);

            var authReponse = new AuthResponseDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiredAt = DateTime.UtcNow.AddMinutes(30)
            };

            return Ok(new Response<AuthResponseDto>(200, "Sign in success", authReponse, true));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var tokenInDb = await repository.GetRefreshTokenAsync(request.RefreshToken);

            if (tokenInDb == null || tokenInDb.Contents.IsRevoked || tokenInDb.Contents.ExpiryDate < DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            //var user = await repository.GetUserByIdAsync(tokenInDb.Contents.);
            var user = new SiginGoogleDto();

            var newAccessToken = jwtService.GenerateToken(user);
            var newRefreshToken = jwtService.GenerateRefreshToken();

            // revoke old token
            await repository.RevokeRefreshTokenAsync(tokenInDb.Contents.UserId);

            // save new one
            await repository.SaveRefreshTokenAsync(user.GoogleId!, newRefreshToken);

            return Ok(new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiredAt = DateTime.UtcNow.AddMinutes(30)
            });
        }
    }
}
