using Microsoft.Data.SqlClient;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class MatchRepository : IMatchRepository
    {
        private readonly IExecuteQuery execute;

        public MatchRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<IEnumerable<GroupedMatchDto>>> GetMatchesAsync(int matchWeek, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetMatches" };
                cmd.Parameters.AddWithValue("@Week", matchWeek);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var matches = new List<MatchDto>();

                if (rdr != null)
                {
                    do
                    {
                        matches.Add(new MatchDto()
                        {
                            MatchId = rdr.SafeGetInt("MatchId"),
                            MatchDate = rdr.SafeGetString("MatchDate"),
                            Matchweek = rdr.SafeGetInt("Matchweek"),
                            LatestMatchWeek = rdr.SafeGetString("LatestMatchWeek"),
                            LatestMatchweekDateRange = rdr.SafeGetString("LatestMatchweekDateRange"),
                            MatchTime = rdr.SafeGetString("MatchTime"),
                            HomeClubId = rdr.SafeGetInt("HomeClubId"),
                            AwayClubId = rdr.SafeGetInt("AwayClubId"),
                            HomeClubName = rdr.SafeGetString("HomeClubName"),
                            AwayClubName = rdr.SafeGetString("AwayClubName"),
                            HomeClubCrest = rdr.SafeGetString("HomeClubCrest"),
                            AwayClubCrest = rdr.SafeGetString("AwayClubCrest"),
                            HomeClubGoal = rdr.SafeGetInt("HomeClubGoal"),
                            AwayClubGoal = rdr.SafeGetInt("AwayClubGoal"),
                            KickoffStatus = rdr.SafeGetString("KickoffStatus"),
                            IsGameFinished = rdr.SafeGetString("IsGameFinished"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                var groupedMatches = matches
                    .GroupBy(m => new
                    {
                        m.Matchweek,
                        m.MatchDate,
                        m.LatestMatchWeek,
                        m.LatestMatchweekDateRange
                    })
                    .Select(g => new GroupedMatchDto
                    {
                        Matchweek = g.Key.Matchweek,
                        MatchDate = g.Key.MatchDate,
                        LatestMatchWeek = g.Key.LatestMatchWeek,
                        LatestMatchweekDateRange = g.Key.LatestMatchweekDateRange,
                        Matches = g.ToList()
                    })
                    .ToList();

                return new Response<IEnumerable<GroupedMatchDto>>(200, "Success", groupedMatches, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedMatchDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedMatchDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }
    }
}
