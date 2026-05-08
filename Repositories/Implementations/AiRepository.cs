using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class AiRepository : IAiRepository
    {
        private readonly IExecuteQuery execute;

        public AiRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<dynamic>> AiQuery(string sql, CancellationToken ct = default)
        {
            try
            {   
                var cmd = new SqlCommand();
                cmd.CommandText = sql;

                var rdr = await execute.ExecuteQueryAsync(cmd);
                var results = new List<Dictionary<string, object>>();
                if (rdr is not null)
                {
                    do
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            row.Add(rdr.GetName(i), rdr.GetValue(i));
                        }
                        results.Add(row);
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<dynamic>(200, "Success", results, true);
            }
            catch (SqlException ex)
            {
                return new Response<dynamic>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<dynamic>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }
    }
}
