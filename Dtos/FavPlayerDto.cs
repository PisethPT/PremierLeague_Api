namespace PremierLeague_Api.Dtos
{
    public class FavPlayerDto
    {
        public int PlayerId { get; set; }
        public string UserId { get; set; }
        public string PlayerName { get; set; }
        public string Photo { get; set; }
        public string ClubName { get; set; }
        public string ClubCrest { get; set; }
        public string ClubTheme { get; set; }
        public bool FollowStatus { get; set; }
    }
}
