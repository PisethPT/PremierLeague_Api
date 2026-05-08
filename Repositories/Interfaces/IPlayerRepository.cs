using PremierLeague_Api.Dtos;
using PremierLeague_Api.Query;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Response<IEnumerable<PlayerDto>>> GetPlayersAsync(PlayerQuery? query = null, CancellationToken ct = default);
        Task<Response<PlayerInfoDetailDto>> GetPlayerInfoAsync(int playerId, CancellationToken ct = default);
        Task<Response<IEnumerable<PlayerInfoDto>>> GetPlayerTeammatesAsync(int playerId, CancellationToken ct = default);
        Task<Response<PlayerClubDto>> GetPlayerClubAsync(int playerId, CancellationToken ct = default);

    }
}
