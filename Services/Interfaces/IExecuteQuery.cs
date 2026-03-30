using Microsoft.Data.SqlClient;
using System.Data;

namespace PremierLeague_Api.Services.Interfaces
{
    public interface IExecuteQuery
    {
        Task<SqlDataReader> ExecuteReaderAsync(SqlCommand cmd, CancellationToken ct = default);
        Task<DataSet> ExecuteDataSetAsync(SqlCommand cmd, CancellationToken ct = default);
        Task<T?> ExecuteScalarAsync<T>(SqlCommand cmd, CancellationToken ct = default);
    }
}
