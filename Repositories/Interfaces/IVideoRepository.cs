using PremierLeague_Api.Dtos;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IVideoRepository
    {
        Task<Response<IEnumerable<GroupedVideosDto>>> GetLatestVideosAsync(CancellationToken ct = default);
        Task<Response<IEnumerable<GroupedVideosDto>>> GetSeriesAsync(CancellationToken ct = default);
        Task<Response<IEnumerable<GroupedVideosDto>>> GetTheArchiveAsync(CancellationToken ct = default);
        Task<Response<IEnumerable<HomeVideoDto>>> GetTheAllVideosAsync(int page = 1, CancellationToken ct = default);
        Task<Response<int>> GetTheAllVideosCountAsync(CancellationToken ct = default);
    }
}
