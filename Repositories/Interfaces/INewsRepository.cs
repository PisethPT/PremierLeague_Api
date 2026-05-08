using PremierLeague_Api.Dtos;
using PremierLeague_Api.Query;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface INewsRepository
    {
        Task<Response<object>> GetNewsAsync(NewsQuery query, CancellationToken ct = default);
    }
}
