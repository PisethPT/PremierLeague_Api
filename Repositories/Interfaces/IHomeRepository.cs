using PremierLeague_Api.Dtos;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IHomeRepository
    {
        Task<Response<IEnumerable<HomeClubNewsDto>>> GetHomeClubNewsAsync(CancellationToken ct = default);
        Task<Response<IEnumerable<GroupedHomeMatchesDto>>> GetHomeMatchesAsync(CancellationToken ct = default);
        Task<Response<IEnumerable<RelatedDto>>> GetHomeNewsAsync(CancellationToken ct = default);
        Task<Response<IEnumerable<GroupedVideoDto>>> GetHomeStoriesNewsAsync(List<string> videosTag, int pageSize = 10, CancellationToken ct = default);
        Task<Response<IEnumerable<GroupedVideoDto>>> GetHomeVideosAsync(List<string> videoCategories ,CancellationToken ct = default);
        Task<Response<IEnumerable<GroupedNewsDto>>> GetHomeNewsAsync(List<string> newsTags, int pageSize = 5, CancellationToken ct = default);
        Task<Response<IEnumerable<HomeNewsDto>>> GetHomeNewsFromTheClubsAsync(CancellationToken ct = default);
        Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomeNewsMulitTopicAsync(List<string> newsTags, int pageSize = 5, CancellationToken ct = default);
        Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomeNewsAsTopicAsync(List<string> newsTags, int pageSize = 5, CancellationToken ct = default);
        Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomePremierLeagueNewOnlyAsync(int pageSize = 5, CancellationToken ct = default);
        Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomeQuizzesNewOnlyAsync(int pageSize = 5, CancellationToken ct = default);
        Task<Response<VideoRelatedGroupDto>> GetHomeVideoViewerAsync(int videoId, CancellationToken ct = default);
        Task<Response<NewsRelatedGroupDto>> GetHomeNewsViewerAsync(int newsId, CancellationToken ct = default);
        Task<Response<IEnumerable<HomeNewsTopicDto>>> GetHomePermierLeagueGameNewsAsync(int pageSize = 8, CancellationToken ct = default);
    }
}
