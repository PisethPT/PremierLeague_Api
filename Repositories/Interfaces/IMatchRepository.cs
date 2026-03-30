using PremierLeague_Api.Dtos;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        Task<Response<IEnumerable<GroupedMatchDto>>> GetMatchesAsync(int matchWeek, CancellationToken ct = default);
    }
}
