using Microsoft.Data.SqlClient;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class TableRepository : ITableRepository
    {
        private readonly IExecuteQuery execute;

        public TableRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }
        public async Task<Response<IEnumerable<TableDto>>> GetTableAsync(int seasonId = 4, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiTable" };
                cmd.Parameters.AddWithValue("@SeasonId", seasonId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var tables = new List<TableDto>();

                if (rdr is not null)
                {
                    do
                    {
                        tables.Add(new TableDto()
                        {
                            Position = rdr.SafeGetInt("Position"),
                            ClubId = rdr.SafeGetInt("ClubId"),
                            ClubName = rdr.SafeGetString("ClubName"),
                            ClubCrest = rdr.SafeGetString("ClubCrest"),
                            Played = rdr.SafeGetInt("Played"),
                            Wins = rdr.SafeGetInt("Wins"),
                            Draws = rdr.SafeGetInt("Draws"),
                            Losses = rdr.SafeGetInt("Losses"),
                            GF = rdr.SafeGetInt("GF"),
                            GA = rdr.SafeGetInt("GA"),
                            GD = rdr.SafeGetInt("GD"),
                            Points = rdr.SafeGetInt("Points"),
                            Form = rdr.SafeGetString("Form"),
                            Next = rdr.SafeGetString("Next"),
                            Qualification = rdr.SafeGetString("Qualification"),
                            PositionStatus = rdr.SafeGetString("PositionStatus"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<TableDto>>(200, "Success", tables, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<TableDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<TableDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }
    }
}
