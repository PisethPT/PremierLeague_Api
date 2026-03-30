using Microsoft.IdentityModel.Tokens;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PremierLeague_Api.Services.Implementations
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;

        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        }

        public string GenerateToken(SiginGoogleDto signInGoogleDto)
        {
            var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, signInGoogleDto.GoogleId!),
            new Claim(ClaimTypes.Email, signInGoogleDto.Email),
            new Claim(ClaimTypes.Name, signInGoogleDto.FirstName)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
