using Microsoft.Data.SqlClient;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Responses;
using PremierLeague_Api.Services.Interfaces;

namespace PremierLeague_Api.Repositories.Implementations
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IExecuteQuery execute;

        public AuthRepository(IExecuteQuery execute)
        {
            this.execute = execute;
        }

        public Task<Response<UserRefreshTokenDto>> GetRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Response<SiginGoogleDto>> OAuthGetUserAccountAsync(string email, string googleId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<bool>> OAuthLoginAccountAlreadyAsync(string email)
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

        public async Task<Response<bool>> OAuthSiginGoogleAccontAsync(SiginGoogleDto siginGoogleDto)
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

        public Task<Response<bool>> RevokeRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Response<bool>> SaveRefreshTokenAsync(string googleId, string newRefreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
