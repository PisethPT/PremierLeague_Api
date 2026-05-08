using PremierLeague_Api.Dtos;
using PremierLeague_Api.Query;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<Response<bool>> LoginAccountAlreadyAsync(string email);
        Task<Response<bool>> SiginGoogleAccontAsync(SiginGoogleDto siginGoogleDto);
        Task<Response<SiginGoogleDto>> GetUserByIdAsync(string userId);
        Task<Response<SiginGoogleDto>> GetUserByEmailAsync(string email);
        Task<Response<bool>> SaveRefreshTokenAsync(string userId, string newRefreshToken);
        Task<Response<UserRefreshTokenDto>> GetRefreshTokenAsync(string refreshToken);
        Task<Response<IEnumerable<GroupedFavClubDto>>> GetFavClubsAsync(string? email, CancellationToken ct = default);
        Task<Response<IEnumerable<FavClubDto>>> GetFavSelectedClubsAsync(string? email, CancellationToken ct = default);
        Task<Response<IEnumerable<FavPlayerDto>>> GetFavPlayersAsync(FavPlayerRequest? request, CancellationToken ct = default);
        Task<Response<bool>> SaveSelectedClubsAsync(FavRequest? request, CancellationToken ct = default);
        Task<Response<bool>> SaveSelectedPlayersAsync(FavRequest? request, CancellationToken ct = default);
        Task<Response<bool>> CheckUserFavoritesAsync(string? email, CancellationToken ct = default);
        Task<Response<GroupedmyPLSettingsDto>> GetmyPLSettingsAsync(string? email, CancellationToken ct = default);
    }
}
