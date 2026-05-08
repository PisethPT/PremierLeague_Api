using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Helper;
using PremierLeague_Api.Query;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly IExecuteQuery execute;

        public PlayerRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public async Task<Response<PlayerClubDto>> GetPlayerClubAsync(int playerId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetPlayerClub" };
                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var value = new PlayerClubDto();

                if (rdr is not null)
                {
                    value.PlayerId = rdr.SafeGetInt("PlayerId");
                    value.ClubId = rdr.SafeGetInt("ClubId");
                }

                return new Response<PlayerClubDto>(200, "Success", value, true);
            }
            catch (SqlException ex)
            {
                return new Response<PlayerClubDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<PlayerClubDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<PlayerInfoDetailDto>> GetPlayerInfoAsync(int playerId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetPlayerInfo" };
                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var player = new PlayerInfoDetailDto();

                if (rdr is not null)
                {
                    player.PlayerId = rdr.SafeGetInt("PlayerId");
                    player.ClubId = rdr.SafeGetInt("ClubId");
                    player.FirstName = rdr.SafeGetString("Firstname");
                    player.LastName = rdr.SafeGetString("Lastname");
                    player.PlayerNumber = rdr.SafeGetString("PlayerNumber");
                    player.Position = rdr.SafeGetString("Position");
                    player.Photo = rdr.SafeGetString("Photo");
                    player.Nationality = rdr.SafeGetString("Nationality");
                    player.PlaceOfBirth = rdr.SafeGetString("PlaceOfBirth");
                    player.PreferredFoot = rdr.SafeGetString("PreferredFoot");
                    player.DateOfBirth = rdr.SafeGetString("DateOfBirth");
                    player.JoinedClub = rdr.SafeGetString("JoinedClub");
                    player.Height = rdr.SafeGetString("Height");
                    player.ClubName = rdr.SafeGetString("ClubName");
                    player.ClubShortName = rdr.SafeGetString("ClubShortName");
                    player.ClubCrest = rdr.SafeGetString("ClubCrest");
                    player.ClubTheme = rdr.SafeGetString("ClubTheme");
                    player.Appearances = rdr.SafeGetInt("Appearances");
                    player.Goals = rdr.SafeGetInt("Goals");
                    player.Assists = rdr.SafeGetInt("Assists");
                }

                return new Response<PlayerInfoDetailDto>(200, "Success", player, true);
            }
            catch (SqlException ex)
            {
                return new Response<PlayerInfoDetailDto>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<PlayerInfoDetailDto>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<PlayerDto>>> GetPlayersAsync(PlayerQuery? query = null, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetPlayers" };
                cmd.Parameters.AddWithValue("@Page", query?.Page);
                cmd.Parameters.AddWithValue("@PageSize", query?.PageSize);
                cmd.Parameters.AddWithValue("@Competition", query?.Competition);
                cmd.Parameters.AddWithValue("@Season", query?.Season);
                cmd.Parameters.AddWithValue("@PositionJson", JsonConvert.SerializeObject(query?.Positions));
                cmd.Parameters.AddWithValue("@ClubIdJson", JsonConvert.SerializeObject(query?.Clubs));
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var clubs = new List<PlayerDto>();

                if (rdr is not null)
                {
                    do
                    {
                        clubs.Add(new PlayerDto()
                        {
                            PlayerId = rdr.SafeGetInt("PlayerId"),
                            ClubId = rdr.SafeGetInt("ClubId"),
                            PlayerName = rdr.SafeGetString("PlayerName"),
                            PlayerPhoto = rdr.SafeGetString("PlayerPhoto"),
                            Position = rdr.SafeGetString("Position"),
                            Nationality = rdr.SafeGetString("Nationality"),
                            ClubName = rdr.SafeGetString("ClubName"),
                            ClubCrest = rdr.SafeGetString("ClubCrest"),
                            ClubTheme = rdr.SafeGetString("ClubTheme")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<PlayerDto>>(200, "Success", clubs, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<PlayerDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<PlayerDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }

        public async Task<Response<IEnumerable<PlayerInfoDto>>> GetPlayerTeammatesAsync(int playerId, CancellationToken ct = default)
        {
            try
            {
                var cmd = new SqlCommand { CommandText = "PL_ApiGetPlayerTeammates" };
                cmd.Parameters.AddWithValue("@PlayerId", playerId);
                var rdr = await execute.ExecuteReaderAsync(cmd);
                var teammates = new List<PlayerInfoDto>();

                if (rdr is not null)
                {
                    do
                    {
                        teammates.Add(new PlayerInfoDto()
                        {
                            PlayerId = rdr.SafeGetInt("PlayerId"),
                            PlayerName = rdr.SafeGetString("PlayerName"),
                            PlayerNumber = rdr.SafeGetString("PlayerNumber"),
                            PlayerPhoto = rdr.SafeGetString("Photo"),
                            Position = rdr.SafeGetString("Position"),
                            ClubTheme = rdr.SafeGetString("ClubTheme")
                        });
                    } while (await rdr.ReadAsync(ct).ConfigureAwait(false));
                }

                return new Response<IEnumerable<PlayerInfoDto>>(200, "Success", teammates, true);
            }
            catch (SqlException ex)
            {
                return new Response<IEnumerable<PlayerInfoDto>>(400, "Database Error: " + ex.Message, null!, false);
            }
            catch (Exception ex)
            {
                return new Response<IEnumerable<PlayerInfoDto>>(500, $"Internal Server Error {ex.Message}", null!, false);
            }
        }
    }
}
