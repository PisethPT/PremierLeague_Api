namespace PremierLeague_Api.Dtos
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiredAt { get; set; }
    }
}
