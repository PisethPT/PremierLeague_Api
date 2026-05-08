using Microsoft.Data.SqlClient;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class SelectListItemRepository : ISelectListItemRepository
    {
        private readonly IExecuteQuery execute;

        public SelectListItemRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<IEnumerable<T>>> SelectListItemAsync<T>(string commandText, Func<SqlDataReader, T> mapFunc, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = commandText };
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var selectListItems = new List<T>();

                if (rdr is not null)
                {
                    do
                    {
                        var item = mapFunc(rdr);
                        selectListItems.Add(item);
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<T>>(200, "Success", selectListItems, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<T>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<T>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<T>>> SelectListItemAsync<T>(string commandText, Dictionary<string, string>? sqlParams, Func<SqlDataReader, T> mapFunc, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = commandText };
                if (sqlParams is not null)
                foreach (var param in sqlParams)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }

                var rdr = await execute.ExecuteReaderAsync(cmd);
                var selectListItems = new List<T>();

                if (rdr is not null)
                {
                    do
                    {
                        var item = mapFunc(rdr);
                        selectListItems.Add(item);
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<T>>(200, "Success", selectListItems, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<T>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<T>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }
    }
}
