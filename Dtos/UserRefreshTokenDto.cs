namespace PremierLeague_Api.Dtos
{
    public class UserRefreshTokenDto
    {
        public string UserId { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string Provider { get; set; } = "PremierLeagueApi";
        public DateTime? ExpiryDate { get; set; }
        public bool IsRevoked { get; set; } = false;
    }
}