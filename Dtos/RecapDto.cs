namespace PremierLeague_Api.Dtos
{
    public class RecapDto
    {
        public int MatchId { get; set; }
        public int HomeClubId { get; set; }
        public int AwayClubId { get; set; }
        public int PlayerId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string HomeClubReportUrl { get; set; }
        public string AwayClubReportUrl { get; set; }
        public string HomeClubSite { get; set; }
        public string AwayClubSite { get; set; }
        public string HomeClubName { get; set; }
        public string HomeClubCrest { get; set; }
        public string HomeClubTheme { get; set; }
        public string AwayClubName { get; set; }
        public string AwayClubCrest { get; set; }
        public string AwayClubTheme { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Photo { get; set; }
        public string Position { get; set; }
        public bool IsHomeClubPlayerManOfTheMatch { get; set; }
    }
}
