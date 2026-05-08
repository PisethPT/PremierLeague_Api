using PremierLeague_Api.Dtos;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IMatchRepository
    {
        Task<Response<IEnumerable<GroupedMatchDto>>> GetMatchesAsync(int matchWeek, CancellationToken ct = default);
        Task<Response<IEnumerable<StoryNewsDto>>> GetMatchStoryAsync(int matchId, CancellationToken ct = default);
        Task<Response<RecapDto>> GetMatchRecapAsync(int matchId, CancellationToken ct = default);
        Task<Response<MatchInfoDetailDto>> GetMatchInfoDetailAsync(int matchId, CancellationToken ct = default);
        Task<Response<IEnumerable<HomeNewsTopicDto>>> GetMatchHighlightAsync(int matchId, CancellationToken ct = default);
        Task<Response<IEnumerable<HomeNewsTopicDto>>> GetMatchRelatedContentAsync(int matchId, CancellationToken ct = default);
        Task<Response<GroupedMatchInfoDto>> GetMatchInfoAsync(int matchId, CancellationToken ct = default);
        Task<Response<LineupDto>> GetMatchLinupAsync(int matchId, CancellationToken ct = default);
    }
}
