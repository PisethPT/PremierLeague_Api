using PremierLeague_Api.Dtos;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<Response<bool>> OAuthLoginAccountAlreadyAsync(string email);
        Task<Response<bool>> OAuthSiginGoogleAccontAsync(SiginGoogleDto siginGoogleDto);
        Task<Response<SiginGoogleDto>> OAuthGetUserAccountAsync(string email, string googleId);
        Task<Response<bool>> SaveRefreshTokenAsync(string googleId, string newRefreshToken);
        Task<Response<UserRefreshTokenDto>> GetRefreshTokenAsync(string refreshToken);
        Task<Response<bool>> RevokeRefreshTokenAsync(string refreshToken);

    }
}
