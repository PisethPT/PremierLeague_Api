using Microsoft.Data.SqlClient;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface ISelectListItemRepository
    {
        Task<Response<IEnumerable<T>>> SelectListItemAsync<T>(string commandText, Func<SqlDataReader, T> mapFunc, CancellationToken ct = default);
        Task<Response<IEnumerable<T>>> SelectListItemAsync<T>(string commandText, Dictionary<string, string>? sqlParams, Func<SqlDataReader, T> mapFunc, CancellationToken ct = default);
    }
}
