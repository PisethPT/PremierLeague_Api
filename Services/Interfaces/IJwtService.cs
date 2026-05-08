using PremierLeague_Api.Dtos;

namespace PremierLeague_Api.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(SiginGoogleDto user);
        string GenerateRefreshToken();
    }
}
