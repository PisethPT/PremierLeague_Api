using Microsoft.Data.SqlClient;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;
using System.Text.Json;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class ClubRepository : IClubRepository
    {
        private readonly IExecuteQuery execute;

        public ClubRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<IEnumerable<ClubDto>>> GetAllTimePremierLeagueClubsAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetAllTimePremierLeagueClubs" };
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var clubs = new List<ClubDto>();

                if (rdr is not null)
                {
                    do
                    {
                        clubs.Add(new ClubDto()
                        {
                            ClubId = rdr.SafeGetInt("ClubId"),
                            ClubName = rdr.SafeGetString("ClubName"),
                            ClubCrest = rdr.SafeGetString("ClubCrest"),
                            ClubTheme = rdr.SafeGetString("ClubTheme"),
                            ClubStadium = rdr.SafeGetString("ClubStadium"),
                            ClubOfficialWebsite = rdr.SafeGetString("ClubOfficialWebsite")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<ClubDto>>(200, "Success", clubs, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<ClubDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<ClubDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<GroupedClubDetailDto>> GetClubDetailAsync(int clubId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiClubDetail";
                cmd.Parameters.AddWithValue("@ClubId", clubId);
                var club = new GroupedClubDetailDto();
                
                using (var rdr = await execute.ExecuteReadersAsync(cmd))
                {
                    if (rdr is null)
                        return new Response<GroupedClubDetailDto>(404, "Club not found", null!, false);

                    if (await rdr.ReadAsync(ct))
                    {
                        club.ClubDetail.ClubId = rdr.SafeGetInt("ClubId");
                        club.ClubDetail.ClubName = rdr.SafeGetString("ClubName");
                        club.ClubDetail.ClubCrest = rdr.SafeGetString("ClubCrest");
                        club.ClubDetail.ClubTheme = rdr.SafeGetString("ClubTheme");
                        club.ClubDetail.Est = rdr.SafeGetInt("Est");
                        club.ClubDetail.ClubStadium = rdr.SafeGetString("ClubStadium");
                        club.ClubDetail.OfficialClubSite = rdr.SafeGetString("OfficialClubSite");
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                        {
                            club.ClubSocialMedias.Add(new ClubSocialMediaDto
                            {
                                Name = rdr.SafeGetString("Name"),
                                FontAwesome = rdr.SafeGetString("FontAwesome"),
                                SocialMediaUrl = rdr.SafeGetString("SocialMediaUrl")
                            });
                        }
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                        {
                            club.StoryNews.Add(new StoryNewsDto
                            {
                                VideoId = rdr.SafeGetInt("VideoId"),
                                Title = rdr.SafeGetString("Title"),
                                VideoTag = rdr.SafeGetString("VideoTag"),
                                Thumbnail = rdr.SafeGetString("Thumbnail"),
                                VideoUrl = rdr.SafeGetString("VideoUrl"),
                                Duration = rdr.SafeGetDecimal("Duration")
                            });
                        }
                    }
                }
               
                return new Response<GroupedClubDetailDto>(200, "Success", club, true);
            }
            catch (SqlException ex)
            {
                return new Response<GroupedClubDetailDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<GroupedClubDetailDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<ClubLastStarting11Dto>> GetClubLastStarting11Async(int clubId, CancellationToken ct = default)
        {
            try
            {
                using var cmd = new SqlCommand("PL_ApiGetClubLastStarting11");
                cmd.Parameters.AddWithValue("@ClubId", clubId);

                var lastStarting11 = new ClubLastStarting11Dto();

                using (var rdr = await execute.ExecuteReadersAsync(cmd, ct))
                {
                    if (rdr is null)
                        return new Response<ClubLastStarting11Dto>(400, "Reader null", null!, false);

                    if (await rdr.ReadAsync(ct))
                    {
                        lastStarting11.LastMatch.MatchId = rdr.SafeGetInt("MatchId");
                        lastStarting11.LastMatch.MatchDate = rdr.SafeGetString("MatchDate");
                        lastStarting11.LastMatch.Matchweek = rdr.SafeGetString("Matchweek");
                        lastStarting11.LastMatch.HomeClubName = rdr.SafeGetString("HomeClubName");
                        lastStarting11.LastMatch.AwayClubName = rdr.SafeGetString("AwayClubName");
                        lastStarting11.LastMatch.HomeClubCrest = rdr.SafeGetString("HomeClubCrest");
                        lastStarting11.LastMatch.AwayClubCrest = rdr.SafeGetString("AwayClubCrest");
                        lastStarting11.LastMatch.HomeClubGoal = rdr.SafeGetInt("HomeClubGoal");
                        lastStarting11.LastMatch.AwayClubGoal = rdr.SafeGetInt("AwayClubGoal");
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct))
                        {
                            lastStarting11.MatchId = rdr.SafeGetInt("MatchId");
                            lastStarting11.ClubFormationId = rdr.SafeGetInt("ClubFormationId");
                            lastStarting11.ClubManager = rdr.SafeGetString("ClubManager");

                            var json = rdr.SafeGetString("ClubLineup");
                            lastStarting11.ClubLineups = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<ClubLineupDto>>(json)! : new List<ClubLineupDto>();
                        };
                    }
                }

                return new Response<ClubLastStarting11Dto>(200, "Success", lastStarting11, true);
            }
            catch (SqlException ex)
            {
                return new Response<ClubLastStarting11Dto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<ClubLastStarting11Dto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }

        }

        public async Task<Response<IEnumerable<MatchesDto>>> GetClubMatchesAsync(int seasonId = 4, int? clubId = null, int? month = null, CancellationToken ct = default)
        {
            try
            {
                using var cmd = new SqlCommand("PL_ApiGetClubMatches");
                cmd.Parameters.AddWithValue("@SeasonId", seasonId);
                cmd.Parameters.AddWithValue("@ClubId", clubId);
                cmd.Parameters.AddWithValue("@Month", month);

                var matches = new List<MatchesDto>();

                using (var rdr = await execute.ExecuteReaderAsync(cmd, ct))
                {
                    if (rdr is null)
                        return new Response<IEnumerable<MatchesDto>>(400, "Reader null", null!, false);

                    if (rdr is not null)
                    {
                        do
                        {
                            matches.Add(new MatchesDto()
                            {
                                MatchId = rdr.SafeGetInt("MatchId"),
                                MatchDate = rdr.SafeGetString("MatchDate"),
                                kickoffTime = rdr.SafeGetString("kickoffTime"),
                                HomeClubName = rdr.SafeGetString("HomeClubName"),
                                AwayClubName = rdr.SafeGetString("AwayClubName"),
                                HomeClubCrest = rdr.SafeGetString("HomeClubCrest"),
                                AwayClubCrest = rdr.SafeGetString("AwayClubCrest"),
                                HomeClubTheme = rdr.SafeGetString("HomeClubTheme"),
                                AwayClubTheme = rdr.SafeGetString("AwayClubTheme"),
                                HomeClubGoal = rdr.SafeGetString("HomeClubGoal"),
                                AwayClubGoal = rdr.SafeGetString("AwayClubGoal"),
                                Competition = rdr.SafeGetString("Competition"),
                                KickoffStatus = rdr.SafeGetString("KickoffStatus"),
                                IsGameFinished = rdr.SafeGetString("IsGameFinished")
                            });
                        } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                    }
                }

                return new Response<IEnumerable<MatchesDto>>(200, "Success", matches, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<MatchesDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<MatchesDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<MonthDto>>> GetClubMonthOfMatchAsync(int? seasonId = 4, int? clubId = null, CancellationToken ct = default)
        {
            try
            {
                using var cmd = new SqlCommand("PL_ApiGetClubMonthOfMatch");
                cmd.Parameters.AddWithValue("@ClubId", clubId);
                cmd.Parameters.AddWithValue("@SeasonId", seasonId);

                var months = new List<MonthDto>();

                using (var rdr = await execute.ExecuteReaderAsync(cmd, ct))
                {
                    if (rdr is null)
                        return new Response<IEnumerable<MonthDto>>(400, "Reader null", null!, false);

                    if (rdr is not null)
                    {
                        do
                        {
                            months.Add(new MonthDto()
                            {
                                MonthNumber = rdr.SafeGetInt("MonthNumber"),
                                MonthName = rdr.SafeGetString("MonthName")
                            });
                        } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                    }
                }

                return new Response<IEnumerable<MonthDto>>(200, "Success", months, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<MonthDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<MonthDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<GroupedFromtheClubAndServiceDto>> GetClubNewsAndServicesAsync(int clubId, CancellationToken ct = default)
        {
            try
            {
                using var cmd = new SqlCommand("PL_ApiClubOverview_ClubNewsAndService");
                cmd.Parameters.AddWithValue("@ClubId", clubId);

                var values = new GroupedFromtheClubAndServiceDto();

                using (var rdr = await execute.ExecuteReadersAsync(cmd, ct))
                {
                    if (rdr is null)
                        return new Response<GroupedFromtheClubAndServiceDto>(400, "Reader null", null!, false);

                    if (await rdr.ReadAsync(ct))
                    {
                        do
                        {
                            values.FromTheClubs.Add(new FromTheClubNewsDto
                            {
                                NewsId = rdr.SafeGetInt("NewsId"),
                                Title = rdr.SafeGetString("Title"),
                                Thumbnail = rdr.SafeGetString("Thumbnail"),
                                ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                                TagName = rdr.SafeGetString("TagName"),
                                ClubCrest = rdr.SafeGetString("ClubCrest"),
                                ClubTheme = rdr.SafeGetString("ClubTheme")
                            });
                        } while (await rdr.ReadAsync(ct));
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct))
                        {
                            values.ClubServices.Add(new ClubServiceDto
                            {
                                ServiceId = rdr.SafeGetInt("ServiceId"),
                                ServiceName = rdr.SafeGetString("ServiceName"),
                                ServiceUrl = rdr.SafeGetString("ServiceUrl")
                            });
                        }
                    }
                }

                return new Response<GroupedFromtheClubAndServiceDto>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<GroupedFromtheClubAndServiceDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<GroupedFromtheClubAndServiceDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<ClubNewsDto>>> GetClubNewsAsync(int? seasonId = 4, int? clubId = null, CancellationToken ct = default)
        {
            try
            {
                using var cmd = new SqlCommand("PL_ApiGetClubNews");
                cmd.Parameters.AddWithValue("@ClubId", clubId);
                cmd.Parameters.AddWithValue("@SeasonId", seasonId);

                var news = new List<ClubNewsDto>();

                using (var rdr = await execute.ExecuteReaderAsync(cmd, ct))
                {
                    if (rdr is null)
                        return new Response<IEnumerable<ClubNewsDto>>(400, "Reader null", null!, false);

                    if (rdr is not null)
                    {
                        do
                        {
                            news.Add(new ClubNewsDto()
                            {
                                NewsId = rdr.SafeGetInt("NewsId"),
                                Title = rdr.SafeGetString("Title"),
                                Thumbnail = rdr.SafeGetString("Thumbnail"),
                                ReferenceUrl = rdr.SafeGetString("ReferenceUrl"),
                                TagName = rdr.SafeGetString("TagName"),
                                ClubCrest = rdr.SafeGetString("ClubCrest"),
                                ClubTheme = rdr.SafeGetString("ClubTheme")
                            });
                        } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                    }
                }

                return new Response<IEnumerable<ClubNewsDto>>(200, "Success", news, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<ClubNewsDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<ClubNewsDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<GroupedClubOverviewNextMatchAndTeamFormDto>> GetClubOverviewNextMatchAndTeamFormAsync(int clubId, CancellationToken ct = default)
        {
            try
            {
                using var cmd = new SqlCommand("PL_ApiClubOverview_NextMatchAndTeamForm");
                cmd.Parameters.AddWithValue("@ClubId", clubId);

                var overviews = new GroupedClubOverviewNextMatchAndTeamFormDto();

                using (var rdr = await execute.ExecuteReadersAsync(cmd, ct))
                {
                    if (rdr is null)
                        return new Response<GroupedClubOverviewNextMatchAndTeamFormDto>(400, "Reader null", null!, false);

                    if (await rdr.ReadAsync(ct))
                    {
                        overviews.NextMatch.MatchId = rdr.SafeGetInt("MatchId");
                        overviews.NextMatch.MatchDate = rdr.SafeGetString("MatchDate");
                        overviews.NextMatch.kickoffTime = rdr.SafeGetString("kickoffTime");
                        overviews.NextMatch.HomeClubName = rdr.SafeGetString("HomeClubName");
                        overviews.NextMatch.AwayClubName = rdr.SafeGetString("AwayClubName");
                        overviews.NextMatch.HomeClubCrest = rdr.SafeGetString("HomeClubCrest");
                        overviews.NextMatch.AwayClubCrest = rdr.SafeGetString("AwayClubCrest");
                        overviews.NextMatch.HomeClubTheme = rdr.SafeGetString("HomeClubTheme");
                        overviews.NextMatch.AwayClubTheme = rdr.SafeGetString("AwayClubTheme");
                        overviews.NextMatch.HomeClubGoal = rdr.SafeGetString("HomeClubGoal");
                        overviews.NextMatch.AwayClubGoal = rdr.SafeGetString("AwayClubGoal");
                        overviews.NextMatch.Competition = rdr.SafeGetString("Competition");
                        overviews.NextMatch.KickoffStatus = rdr.SafeGetString("KickoffStatus");
                        overviews.NextMatch.IsGameFinished = rdr.SafeGetString("IsGameFinished");
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct))
                        {
                            overviews.ClubDetail.ClubId = rdr.SafeGetInt("ClubId");
                            overviews.ClubDetail.ClubName = rdr.SafeGetString("ClubName");
                            overviews.ClubDetail.ClubCrest = rdr.SafeGetString("ClubCrest");
                            overviews.ClubDetail.ClubTheme = rdr.SafeGetString("ClubTheme");
                        }
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct))
                        {
                            overviews.TeamFormPrevious.Add(new TeamFormPreviousDto
                            {
                                MatchId = rdr.SafeGetInt("MatchId"),
                                HomeClubName = rdr.SafeGetString("HomeClubName"),
                                AwayClubName = rdr.SafeGetString("AwayClubName"),
                                MatchDate = rdr.SafeGetString("MatchDate"),
                                Matchweek = rdr.SafeGetString("Matchweek"),
                                HomeClubGoal = rdr.SafeGetInt("HomeClubGoal"),
                                OtherClubName = rdr.SafeGetString("OtherClubName"),
                                OtherClubCrest = rdr.SafeGetString("OtherClubCrest"),
                                OtherClubGoal = rdr.SafeGetInt("OtherClubGoal"),
                                IsHomeClub = rdr.SafeGetString("IsHomeClub"),
                                MatchResult = rdr.SafeGetString("MatchResult"),
                            });
                        }
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct))
                        {
                            overviews.TeamFormUpcoming.Add(new TeamFormUpcomingDto
                            {
                                MatchId = rdr.SafeGetInt("MatchId"),
                                HomeClubName = rdr.SafeGetString("HomeClubName"),
                                AwayClubName = rdr.SafeGetString("AwayClubName"),
                                MatchDate = rdr.SafeGetString("MatchDate"),
                                Matchweek = rdr.SafeGetString("Matchweek"),
                                OtherClubName = rdr.SafeGetString("OtherClubName"),
                                OtherClubCrest = rdr.SafeGetString("OtherClubCrest"),
                                IsHomeClub = rdr.SafeGetString("IsHomeClub"),
                            });
                        }
                    }
                }

                return new Response<GroupedClubOverviewNextMatchAndTeamFormDto>(200, "Success", overviews, true);
            }
            catch (SqlException ex)
            {
                return new Response<GroupedClubOverviewNextMatchAndTeamFormDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<GroupedClubOverviewNextMatchAndTeamFormDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<GroupedPlayerSquadDto>>> GetClubSquadAsync(int seasonId = 4, int? clubId = null, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetClubSquad" };
                cmd.Parameters.AddWithValue("@ClubId", clubId);
                cmd.Parameters.AddWithValue("@SeasonId", seasonId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var players = new List<PlayerDto>();

                if (rdr != null)
                {
                    do
                    {
                        players.Add(new PlayerDto()
                        {
                            PlayerId = rdr.SafeGetInt("PlayerId"),
                            ClubId = rdr.SafeGetInt("ClubId"),
                            PlayerName = rdr.SafeGetString("PlayerName"),
                            PlayerPhoto = rdr.SafeGetString("PlayerPhoto"),
                            Position = rdr.SafeGetString("Position"),
                            PlayerNumber = rdr.SafeGetInt("PlayerNumber"),
                            Nationality = rdr.SafeGetString("Nationality"),
                            ClubName = rdr.SafeGetString("ClubName"),
                            ClubCrest = rdr.SafeGetString("ClubCrest"),
                            ClubTheme = rdr.SafeGetString("ClubTheme"),
                            PositionLabel = rdr.SafeGetString("PositionLabel")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                var clubSquad = players
                    .GroupBy(m => m.PositionLabel)
                    .Select(g => new GroupedPlayerSquadDto
                    {
                        PositionLabel = g.Key,
                        Players = g.ToList()
                    });

                return new Response<IEnumerable<GroupedPlayerSquadDto>>(200, "Success", clubSquad, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedPlayerSquadDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedPlayerSquadDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<TableDto>>> GetClubTableAsync(int? seasonId = 4, int? clubId = null, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiClubRankInTheTable" };
                cmd.Parameters.AddWithValue("@SeasonId", seasonId);
                cmd.Parameters.AddWithValue("@ClubId", clubId);
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

        public async Task<Response<IEnumerable<HomeVideoDto>>> GetClubVideosAsync(int? seasonId = 4, int? clubId = null, CancellationToken ct = default)
        {
            try
            {
                using var cmd = new SqlCommand("PL_ApiGetClubVideos");
                cmd.Parameters.AddWithValue("@ClubId", clubId);
                cmd.Parameters.AddWithValue("@SeasonId", seasonId);

                var videos = new List<HomeVideoDto>();

                using (var rdr = await execute.ExecuteReaderAsync(cmd, ct))
                {
                    if (rdr is null)
                        return new Response<IEnumerable<HomeVideoDto>>(400, "Reader null", null!, false);

                    if (rdr is not null)
                    {
                        do
                        {
                            videos.Add(new HomeVideoDto()
                            {
                                VideoId = rdr.SafeGetInt("VideoId"),
                                Title = rdr.SafeGetString("Title"),
                                VideoTag = rdr.SafeGetString("VideoTag"),
                                Thumbnail = rdr.SafeGetString("Thumbnail"),
                                VideoUrl = rdr.SafeGetString("VideoUrl")
                            });
                        } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                    }
                }

                return new Response<IEnumerable<HomeVideoDto>>(200, "Success", videos, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<HomeVideoDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<HomeVideoDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<PrimaryFormationDto>>> GetFormationAsync(CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiFormation" };
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var clubs = new List<PrimaryFormationDto>();

                if (rdr is not null)
                {
                    do
                    {
                        clubs.Add(new PrimaryFormationDto()
                        {
                            FormationId = rdr.SafeGetInt("FormationId"),
                            Formation = rdr.SafeGetString("Formation"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<PrimaryFormationDto>>(200, "Success", clubs, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<PrimaryFormationDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<PrimaryFormationDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<ClubDto>>> GetSeasonClubsAsync(int? currentSeasonId = null, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetSeasonClubs" };
                cmd.Parameters.AddWithValue("@CurrentSeasonId", currentSeasonId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var clubs = new List<ClubDto>();

                if (rdr is not null)
                {
                    do
                    {
                        clubs.Add(new ClubDto()
                        {
                            ClubId = rdr.SafeGetInt("ClubId"),
                            SeasonId = rdr.SafeGetInt("SeasonId"),
                            ClubName = rdr.SafeGetString("ClubName"),
                            ClubCrest = rdr.SafeGetString("ClubCrest"),
                            ClubTheme = rdr.SafeGetString("ClubTheme"),
                            ClubOfficialWebsite = rdr.SafeGetString("ClubOfficialWebsite")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<ClubDto>>(200, "Success", clubs, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<ClubDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<ClubDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

    }
}
