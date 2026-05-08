namespace PremierLeague_Api.Dtos
{
    public class FavClubDto
    {
        public int ClubId { get; set; }
        public string UserId { get; set; }
        public string ClubName { get; set; }
        public string ClubCrest { get; set; }
        public string ClubTheme { get; set; }
        public bool FollowStatus { get; set; }
        public string MyClubLabel { get; set; }
    }
}
