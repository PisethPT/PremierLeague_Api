namespace PremierLeague_Api.Dtos
{
    public class ClubDto
    {
        public int ClubId { get; set; }
        public int SeasonId { get; set; }
        public string ClubName { get; set; }
        public string ClubCrest { get; set; }
        public string ClubTheme { get; set; }
        public string ClubStadium { get; set; }
        public string ClubOfficialWebsite { get; set; }
    }
}
