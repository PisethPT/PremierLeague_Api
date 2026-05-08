using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IAiRepository
    {
        Task<Response<dynamic>> AiQuery(string sql, CancellationToken ct = default);
    }
}
