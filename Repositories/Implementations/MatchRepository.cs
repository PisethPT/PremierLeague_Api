using Microsoft.Data.SqlClient;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;
using System.Text.Json;

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

        public async Task<Response<IEnumerable<HomeNewsTopicDto>>> GetMatchHighlightAsync(int matchId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetMatchHighlight" };
                cmd.Parameters.AddWithValue("@MatchId", matchId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var value = new List<HomeNewsTopicDto>();

                if (rdr is not null)
                {
                    do
                    {
                        value.Add(new HomeNewsTopicDto()
                        {
                            TopicId = rdr.SafeGetInt("TopicId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            TopicTag = rdr.SafeGetString("TopicTag"),
                            IsVideo = rdr.SafeGetBoolean("IsVideo"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<HomeNewsTopicDto>>(200, "Success", value, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<GroupedMatchInfoDto>> GetMatchInfoAsync(int matchId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetMatchInfo";
                cmd.Parameters.AddWithValue("@MatchId", matchId);
                var values = new GroupedMatchInfoDto();

                using (var rdr = await execute.ExecuteReadersAsync(cmd))
                {
                    if (rdr is null)
                        return new Response<GroupedMatchInfoDto>(404, "Club not found", null!, false);

                    if (await rdr.ReadAsync(ct))
                    {
                        values.MatchDetail.Kickoff = rdr.SafeGetString("Kickoff");
                        values.MatchDetail.Stadium = rdr.SafeGetString("Stadium");
                        values.MatchDetail.Attendance = rdr.SafeGetString("Attendance");

                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                        {
                            values.MatchOfficials.Add(new MatchOfficialDto
                            {
                                RefereeRole = rdr.SafeGetString("RefereeRole"),
                                RefereeName = rdr.SafeGetString("RefereeName"),
                            });
                        }
                    }
                }

                return new Response<GroupedMatchInfoDto>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<GroupedMatchInfoDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<GroupedMatchInfoDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<MatchInfoDetailDto>> GetMatchInfoDetailAsync(int matchId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetMatchDetail";
                cmd.Parameters.AddWithValue("@MatchId", matchId);
                var values = new MatchInfoDetailDto();

                using (var rdr = await execute.ExecuteReadersAsync(cmd))
                {
                    if (rdr is null)
                        return new Response<MatchInfoDetailDto>(404, "Club not found", null!, false);

                    if (await rdr.ReadAsync(ct))
                    {
                        values.MatchInfo.MatchId = rdr.SafeGetInt("MatchId");
                        values.MatchInfo.HomeClubId = rdr.SafeGetInt("HomeClubId");
                        values.MatchInfo.AwayClubId = rdr.SafeGetInt("AwayClubId");
                        values.MatchInfo.MatchDate = rdr.SafeGetString("MatchDate");
                        values.MatchInfo.KickoffTime = rdr.SafeGetString("KickoffTime");
                        values.MatchInfo.HomeClubName = rdr.SafeGetString("HomeClubName");
                        values.MatchInfo.HomeClubCrest = rdr.SafeGetString("HomeClubCrest");
                        values.MatchInfo.HomeClubTheme = rdr.SafeGetString("HomeClubTheme");
                        values.MatchInfo.HomeClubGoal = rdr.SafeGetInt("HomeClubGoal");
                        values.MatchInfo.AwayClubName = rdr.SafeGetString("AwayClubName");
                        values.MatchInfo.AwayClubCrest = rdr.SafeGetString("AwayClubCrest");
                        values.MatchInfo.AwayClubTheme = rdr.SafeGetString("AwayClubTheme");
                        values.MatchInfo.AwayClubGoal = rdr.SafeGetInt("AwayClubGoal");
                        values.MatchInfo.Competition = rdr.SafeGetString("Competition");
                        values.MatchInfo.Stadium = rdr.SafeGetString("Stadium");
                        values.MatchInfo.Referee = rdr.SafeGetString("Referee");
                        values.MatchInfo.Matchweek = rdr.SafeGetString("Matchweek");
                        values.MatchInfo.MatchInfo = rdr.SafeGetString("MatchInfo");
                        values.MatchInfo.KickoffStatusDisplay = rdr.SafeGetString("KickoffStatusDisplay");
                        values.MatchInfo.IsGameFinished = rdr.SafeGetString("IsGameFinished");
                        
                    }

                    //if (await rdr.NextResultAsync(ct))
                    //{
                    //    while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                    //    {
                    //        club.ClubSocialMedias.Add(new ClubSocialMediaDto
                    //        {
                    //            Name = rdr.SafeGetString("Name"),
                    //            FontAwesome = rdr.SafeGetString("FontAwesome"),
                    //            SocialMediaUrl = rdr.SafeGetString("SocialMediaUrl")
                    //        });
                    //    }
                    //}
                }

                return new Response<MatchInfoDetailDto>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<MatchInfoDetailDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<MatchInfoDetailDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<LineupDto>> GetMatchLinupAsync(int matchId, CancellationToken ct = default)
        {
            try
            {
                using var cmd = new SqlCommand("PL_ApiGetMatchLineup");
                cmd.Parameters.AddWithValue("@MatchId", matchId);

                var values = new LineupDto();

                using (var rdr = await execute.ExecuteReadersAsync(cmd, ct))
                {
                    if (rdr is null)
                        return new Response<LineupDto>(400, "Reader null", null!, false);

                    if (await rdr.ReadAsync(ct))
                    {
                        
                        do{
                            values.MatchId = rdr.SafeGetInt("MatchId");
                            values.HomeClubFormationId = rdr.SafeGetInt("HomeClubFormationId");
                            values.HomeClubFormation = rdr.SafeGetString("HomeClubFormation");
                            values.HomeClubShortName = rdr.SafeGetString("HomeClubShortName");
                            values.HomeClubCrest = rdr.SafeGetString("HomeClubCrest");
                            values.HomeClubManager = rdr.SafeGetString("HomeClubManager");
                            values.HomeClubTheme = rdr.SafeGetString("HomeClubTheme");
                            values.AwayClubFormationId = rdr.SafeGetInt("AwayClubFormationId");
                            values.AwayClubFormation = rdr.SafeGetString("AwayClubFormation");
                            values.AwayClubShortName = rdr.SafeGetString("AwayClubShortName");
                            values.AwayClubCrest = rdr.SafeGetString("AwayClubCrest");
                            values.AwayClubManager = rdr.SafeGetString("AwayClubManager");
                            values.AwayClubTheme = rdr.SafeGetString("AwayClubTheme");

                            var homeClubLineupJson = rdr.SafeGetString("HomeClubLineup");
                            var homeClubSubstitutesJson = rdr.SafeGetString("HomeClubSubstitution");

                            values.HomeClubLineups = !string.IsNullOrEmpty(homeClubLineupJson) ? JsonSerializer.Deserialize<List<ClubLineupDto>>(homeClubLineupJson)! : new List<ClubLineupDto>();
                            values.HomeClubSubstitutes = !string.IsNullOrEmpty(homeClubSubstitutesJson) ? JsonSerializer.Deserialize<List<ClubLineupDto>>(homeClubSubstitutesJson)! : new List<ClubLineupDto>();
                            var awayClubLineupJson = rdr.SafeGetString("AwayClubLineup");
                            var awayClubSubstitutesJson = rdr.SafeGetString("AwayClubSubstitution");
                            values.AwayClubLineups = !string.IsNullOrEmpty(awayClubLineupJson) ? JsonSerializer.Deserialize<List<ClubLineupDto>>(awayClubLineupJson)! : new List<ClubLineupDto>();
                            values.AwayClubSubstitutes = !string.IsNullOrEmpty(awayClubSubstitutesJson) ? JsonSerializer.Deserialize<List<ClubLineupDto>>(awayClubSubstitutesJson)! : new List<ClubLineupDto>();
                        } while (await rdr.ReadAsync(ct));
                    }
                }

                return new Response<LineupDto>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<LineupDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<LineupDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }

        }

        public async Task<Response<RecapDto>> GetMatchRecapAsync(int matchId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetRecap_ReportAndPlayerOfTheMatch" };
                cmd.Parameters.AddWithValue("@MatchId", matchId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var value = new RecapDto();

                if (rdr is not null)
                {
                    value.MatchId = rdr.SafeGetInt("MatchId");
                    value.HomeClubId = rdr.SafeGetInt("HomeClubId");
                    value.AwayClubId = rdr.SafeGetInt("AwayClubId");
                    value.PlayerId = rdr.SafeGetInt("PlayerId");
                    value.Title = rdr.SafeGetString("Title");
                    value.Content = rdr.SafeGetString("Content");
                    value.HomeClubReportUrl = rdr.SafeGetString("HomeClubReportUrl");
                    value.AwayClubReportUrl = rdr.SafeGetString("AwayClubReportUrl");
                    value.HomeClubSite = rdr.SafeGetString("HomeClubSite");
                    value.AwayClubSite = rdr.SafeGetString("AwayClubSite");
                    value.HomeClubName = rdr.SafeGetString("HomeClubName");
                    value.HomeClubCrest = rdr.SafeGetString("HomeClubCrest");
                    value.HomeClubTheme = rdr.SafeGetString("HomeClubTheme");
                    value.AwayClubName = rdr.SafeGetString("AwayClubName");
                    value.AwayClubCrest = rdr.SafeGetString("AwayClubCrest");
                    value.AwayClubTheme = rdr.SafeGetString("AwayClubTheme");
                    value.FirstName = rdr.SafeGetString("FirstName");
                    value.LastName = rdr.SafeGetString("LastName");
                    value.Photo = rdr.SafeGetString("Photo");
                    value.Position = rdr.SafeGetString("Position");
                    value.IsHomeClubPlayerManOfTheMatch = rdr.SafeGetBoolean("IsHomeClubPlayerManOfTheMatch");
                }

                return new Response<RecapDto>(200, "Success", value, true);
            }
            catch (SqlException ex)
            {
                return new Response<RecapDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<RecapDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<HomeNewsTopicDto>>> GetMatchRelatedContentAsync(int matchId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetMatchRelatedContent" };
                cmd.Parameters.AddWithValue("@MatchId", matchId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var value = new List<HomeNewsTopicDto>();

                if (rdr is not null)
                {
                    do
                    {
                        value.Add(new HomeNewsTopicDto()
                        {
                            TopicId = rdr.SafeGetInt("TopicId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            TopicTag = rdr.SafeGetString("TopicTag"),
                            IsVideo = rdr.SafeGetBoolean("IsVideo"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<HomeNewsTopicDto>>(200, "Success", value, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeNewsTopicDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<StoryNewsDto>>> GetMatchStoryAsync(int matchId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetMatchStory" };
                cmd.Parameters.AddWithValue("@MatchId", matchId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var value = new List<StoryNewsDto>();

                if (rdr is not null)
                {
                    do
                    {
                        value.Add(new StoryNewsDto()
                        {
                            VideoId = rdr.SafeGetInt("VideoId"),
                            Title = rdr.SafeGetString("Title"),
                            Thumbnail = rdr.SafeGetString("Thumbnail"),
                            VideoTag = rdr.SafeGetString("VideoTag"),
                            VideoUrl = rdr.SafeGetString("VideoUrl"),
                            Duration = rdr.SafeGetDecimal("Duration")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<StoryNewsDto>>(200, "Success", value, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<StoryNewsDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<StoryNewsDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }
    }
}
