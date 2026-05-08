using Azure.Core;
using Microsoft.Data.SqlClient;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Query;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;
using System.Text.Json;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IExecuteQuery execute;

        public AuthRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<UserRefreshTokenDto>> GetRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiAspNetUser_GetRefreshToken";
                cmd.Parameters.AddWithValue("@Token", refreshToken);

                var rdr = await execute.ExecuteReaderAsync(cmd);

                if (rdr is null)
                    return new Response<UserRefreshTokenDto>(404, "Invalid Token", null!, false);

                var values = new UserRefreshTokenDto()
                {
                    UserId = rdr.SafeGetString("UserId"),
                    RefreshToken = rdr.SafeGetString("RefreshToken"),
                    Provider = rdr.SafeGetString("Provider")
                };

                return new Response<UserRefreshTokenDto>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<UserRefreshTokenDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<UserRefreshTokenDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<SiginGoogleDto>> GetUserByEmailAsync(string email)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiAspNetUser_GetUserByEmail";
                cmd.Parameters.AddWithValue("@Email", email);

                var rdr = await execute.ExecuteReaderAsync(cmd);

                if (rdr is null)
                    return new Response<SiginGoogleDto>(404, "Not Found", null!, false);

                var values = new SiginGoogleDto()
                {
                    UserId = rdr.SafeGetString("UserId"),
                    Email = rdr.SafeGetString("Email"),
                    FirstName = rdr.SafeGetString("FirstName"),
                    LastName = rdr.SafeGetString("LastName"),
                    PhotoUrl = rdr.SafeGetString("PhotoUrl")
                };

                return new Response<SiginGoogleDto>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<SiginGoogleDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<SiginGoogleDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<bool>> LoginAccountAlreadyAsync(string email)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiIsLoginAccountAlready";
                cmd.Parameters.AddWithValue("@Email", email);

                bool reponse = await execute.ExecuteScalarAsync<bool>(cmd);

                return new Response<bool>(200, "Success", reponse, true);
            }
            catch (SqlException ex)
            {
                return new Response<bool>(400, "Database Error: " + ex.Message, false, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(500, $"Internal Server Error {ex.Message}", false, false);
            }
        }

        public async Task<Response<bool>> SiginGoogleAccontAsync(SiginGoogleDto siginGoogleDto)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiSiginGoogleAcount";
                cmd.Parameters.AddWithValue("@FirstName", siginGoogleDto.FirstName);
                cmd.Parameters.AddWithValue("@LastName", siginGoogleDto.LastName);
                cmd.Parameters.AddWithValue("@Email", siginGoogleDto.Email);
                cmd.Parameters.AddWithValue("@Photo", siginGoogleDto.PhotoUrl);

                bool reponse = await execute.ExecuteScalarAsync<bool>(cmd) ? true: false;

                return new Response<bool>(200, "Success", reponse, true);
            }
            catch (SqlException ex)
            {
                return new Response<bool>(400, "Database Error: " + ex.Message, false, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(500, $"Internal Server Error {ex.Message}", false, false);
            }
        }

        public async Task<Response<bool>> SaveRefreshTokenAsync(string userId, string newRefreshToken)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiAspNetUser_SaveRefreshToken";
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Value", newRefreshToken);

                await execute.ExecuteScalarAsync<bool>(cmd);

                return new Response<bool>(200, "Success", true, true);
            }
            catch (SqlException ex)
            {
                return new Response<bool>(400, "Database Error: " + ex.Message, false, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(500, $"Internal Server Error {ex.Message}", false, false);
            }
        }

        public async Task<Response<SiginGoogleDto>> GetUserByIdAsync(string userId)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiAspNetUser_GetUserById";
                cmd.Parameters.AddWithValue("@UserId", userId);

                var rdr = await execute.ExecuteReaderAsync(cmd);

                if (rdr is null)
                    return new Response<SiginGoogleDto>(404, "User not found", null!, false);

                var values = new SiginGoogleDto()
                {
                    UserId = rdr.SafeGetString("UserId"),
                    Email = rdr.SafeGetString("Email"),
                    FirstName = rdr.SafeGetString("FirstName"),
                    LastName = rdr.SafeGetString("LastName"),
                    PhotoUrl = rdr.SafeGetString("PhotoUrl")
                };

                return new Response<SiginGoogleDto>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<SiginGoogleDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<SiginGoogleDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<GroupedFavClubDto>>> GetFavClubsAsync(string? email, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiMyClubs";
                cmd.Parameters.AddWithValue("@Email", email);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var values = new List<FavClubDto>();

                if (rdr != null)
                {
                    do
                    {
                        values.Add(new FavClubDto()
                        {
                            ClubId = rdr.SafeGetInt("ClubId"),
                            UserId = rdr.SafeGetString("UserId"),
                            ClubName = rdr.SafeGetString("ClubName"),
                            ClubCrest = rdr.SafeGetString("ClubCrest"),
                            ClubTheme = rdr.SafeGetString("ClubTheme"),
                            FollowStatus = rdr.SafeGetBoolean("FollowStatus"),
                            MyClubLabel = rdr.SafeGetString("MyClubLabel")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                var grouped = values
                    .GroupBy(m => m.MyClubLabel)
                    .Select(g => new GroupedFavClubDto
                    {
                        MyClubLabel = g.Key,
                        Clubs = g.ToList()
                    });

                return new Response<IEnumerable<GroupedFavClubDto>>(200, "Success", grouped, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<GroupedFavClubDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<GroupedFavClubDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<FavPlayerDto>>> GetFavPlayersAsync(FavPlayerRequest? request, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiMyPlayers";
                cmd.Parameters.AddWithValue("@Email", request?.Email);
                cmd.Parameters.AddWithValue("@PlayerName", request?.PlayerName);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var values = new List<FavPlayerDto>();

                if (rdr != null)
                {
                    do
                    {
                        values.Add(new FavPlayerDto()
                        {
                            PlayerId = rdr.SafeGetInt("PlayerId"),
                            UserId = rdr.SafeGetString("UserId"),
                            PlayerName = rdr.SafeGetString("PlayerName"),
                            Photo = rdr.SafeGetString("Photo"),
                            ClubName = rdr.SafeGetString("ClubName"),
                            ClubCrest = rdr.SafeGetString("ClubCrest"),
                            ClubTheme = rdr.SafeGetString("ClubTheme"),
                            FollowStatus = rdr.SafeGetBoolean("FollowStatus"),
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }


                return new Response<IEnumerable<FavPlayerDto>>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<FavPlayerDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<FavPlayerDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<bool>> SaveSelectedClubsAsync(FavRequest? request, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiSeletedMyClubs";
                cmd.Parameters.AddWithValue("@Email", request?.Email);
                cmd.Parameters.AddWithValue("@ClubJson", JsonSerializer.Serialize(request?.JsonData));

                bool reponse = await execute.ExecuteScalarAsync<bool>(cmd) ? true : false;

                return new Response<bool>(200, "Success", reponse, true);
            }
            catch (SqlException ex)
            {
                return new Response<bool>(400, "Database Error: " + ex.Message, false, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(500, $"Internal Server Error {ex.Message}", false, false);
            }
        }

        public async Task<Response<bool>> SaveSelectedPlayersAsync(FavRequest? request, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiSeletedMyPlayers";
                cmd.Parameters.AddWithValue("@Email", request?.Email);
                cmd.Parameters.AddWithValue("@ClubJson", JsonSerializer.Serialize(request?.JsonData));

                bool reponse = await execute.ExecuteScalarAsync<bool>(cmd) ? true : false;

                return new Response<bool>(200, "Success", reponse, true);
            }
            catch (SqlException ex)
            {
                return new Response<bool>(400, "Database Error: " + ex.Message, false, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(500, $"Internal Server Error {ex.Message}", false, false);
            }
        }

        public async Task<Response<IEnumerable<FavClubDto>>> GetFavSelectedClubsAsync(string? email, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetSeletedMyPlayers";
                cmd.Parameters.AddWithValue("@Email", email);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var values = new List<FavClubDto>();

                if (rdr != null)
                {
                    do
                    {
                        values.Add(new FavClubDto()
                        {
                            ClubId = rdr.SafeGetInt("ClubId"),
                            UserId = rdr.SafeGetString("UserId"),
                            ClubName = rdr.SafeGetString("ClubName"),
                            ClubCrest = rdr.SafeGetString("ClubCrest"),
                            ClubTheme = rdr.SafeGetString("ClubTheme"),
                            FollowStatus = rdr.SafeGetBoolean("FollowStatus"),
                            MyClubLabel = rdr.SafeGetString("MyClubLabel")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }


                return new Response<IEnumerable<FavClubDto>>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<FavClubDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<FavClubDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<bool>> CheckUserFavoritesAsync(string? email, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiCheckUserFavarites";
                cmd.Parameters.AddWithValue("@Email", email);

                bool reponse = await execute.ExecuteScalarAsync<bool>(cmd) ? true : false;

                return new Response<bool>(200, "Success", reponse, true);
            }
            catch (SqlException ex)
            {
                return new Response<bool>(400, "Database Error: " + ex.Message, false, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>(500, $"Internal Server Error {ex.Message}", false, false);
            }
        }

        public async Task<Response<GroupedmyPLSettingsDto>> GetmyPLSettingsAsync(string? email, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand();
                cmd.CommandText = "PL_ApiGetmyPLSettings";
                cmd.Parameters.AddWithValue("@Email", email);
                var values = new GroupedmyPLSettingsDto();

                using (var rdr = await execute.ExecuteReadersAsync(cmd))
                {
                    if (rdr is null)
                        return new Response<GroupedmyPLSettingsDto>(404, "Email not found", null!, false);

                    if (await rdr.ReadAsync(ct))
                    {
                        do
                        {

                            values.Info.Add(new myPLSettingsDto
                            {
                                Hello = rdr.SafeGetString("Hello"),
                                FavoriteClub = rdr.SafeGetString("FavoriteClub"),
                                ClubCrest = rdr.SafeGetString("ClubCrest"),
                            });
                        } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                        {
                            values.Matches.Add(new MatchesDto()
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
                        }
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                        {
                            values.FollowingClubs.Add(new myPLSettings_FollowingClubDto
                            {
                                ClubName = rdr.SafeGetString("ClubName"),
                                ClubCrest = rdr.SafeGetString("ClubCrest"),
                                ClubTheme = rdr.SafeGetString("ClubTheme"),
                            });
                        }
                    }

                    if (await rdr.NextResultAsync(ct))
                    {
                        while (await rdr.ReadAsync(ct).ConfigureAwait(false))
                        {
                            values.FollowingPlayers.Add(new myPLSettings_FollowingPlayerDto
                            {
                                ClubName = rdr.SafeGetString("ClubName"),
                                ClubCrest = rdr.SafeGetString("ClubCrest"),
                                ClubTheme = rdr.SafeGetString("ClubTheme"),
                                PlayerName = rdr.SafeGetString("PlayerName"),
                                Photo = rdr.SafeGetString("Photo"),
                            });
                        }
                    }
                }

                return new Response<GroupedmyPLSettingsDto>(200, "Success", values, true);
            }
            catch (SqlException ex)
            {
                return new Response<GroupedmyPLSettingsDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<GroupedmyPLSettingsDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }
    }
}
