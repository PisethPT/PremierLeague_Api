using Microsoft.Data.SqlClient;
using PremierLeague_Api.Data;
using PremierLeague_Api.Services.Interfaces;
using System.Data;

namespace PremierLeague_Api.Services.Implementations
{
    public class ExecuteQuery : IExecuteQuery
    {
        public async Task<SqlDataReader> ExecuteReaderAsync(SqlCommand cmd, CancellationToken ct = default)
        {
            var conn = await AppDbContext.Instance.GetOpenConnectionAsync(ct).ConfigureAwait(false);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, ct).ConfigureAwait(false);
                if (await rdr.ReadAsync(ct).ConfigureAwait(false))
                    return rdr;

                rdr.Close();
                return null!;
            }
            catch (SqlException ex) when (ex.Number == 500000)
            {
                conn.Dispose();
                return null!;
            }
        }

        public async Task<SqlDataReader> ExecuteReadersAsync(SqlCommand cmd, CancellationToken ct = default)
        {
            var conn = await AppDbContext.Instance.GetOpenConnectionAsync(ct).ConfigureAwait(false);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                return await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, ct).ConfigureAwait(false);
            }
            catch (SqlException ex) when (ex.Number == 500000)
            {
                conn.Dispose();
                return null!;
            }
        }

        public async Task<T?> ExecuteScalarAsync<T>(SqlCommand cmd, CancellationToken ct = default)
        {
            await using var conn = await AppDbContext.Instance.GetOpenConnectionAsync(ct).ConfigureAwait(false);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                var scalar = await cmd.ExecuteScalarAsync(ct).ConfigureAwait(false);
                return scalar == null || scalar == DBNull.Value ? default : (T)Convert.ChangeType(scalar, typeof(T))!;
            }
            catch (SqlException ex) when (ex.Number == 500000)
            {
                conn.Dispose();
                return default;
            }
        }

        public async Task<DataSet> ExecuteDataSetAsync(SqlCommand cmd, CancellationToken ct = default)
        {
            var conn = await AppDbContext.Instance.GetOpenConnectionAsync(ct).ConfigureAwait(false);
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                var ds = new DataSet();

                using (var adapter = new SqlDataAdapter(cmd))
                {
                    await Task.Run(() => adapter.Fill(ds), ct);
                }

                return ds.Tables.Count > 0 ? ds : null!;
            }
            catch (SqlException ex) when (ex.Number == 500000)
            {
                conn.Dispose();
                return null!;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<SqlDataReader> ExecuteQueryAsync(SqlCommand cmd, CancellationToken ct = default)
        {
            var conn = await AppDbContext.Instance.GetOpenConnectionAsync(ct).ConfigureAwait(false);
            cmd.Connection = conn;

            try
            {
                var rdr = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection, ct).ConfigureAwait(false);

                if (await rdr.ReadAsync(ct).ConfigureAwait(false))
                    return rdr;

                rdr.Close();
                return null!;
            }
            catch (SqlException ex) when (ex.Number == 500000)
            {
                conn.Dispose();
                return default!;
            }
        }
    }
}
