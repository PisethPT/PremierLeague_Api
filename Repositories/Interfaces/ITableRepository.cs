using PremierLeague_Api.Dtos;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface ITableRepository
    {
        Task<Response<IEnumerable<TableDto>>> GetTableAsync(int seasonId = 4, CancellationToken ct = default);
    }
}
