namespace PremierLeague_Api.Dtos
{
    public class TeamFormUpcomingDto
    {
        public int MatchId { get; set; }
        public string HomeClubName { get; set; }
        public string AwayClubName { get; set; }
        public string MatchDate { get; set; }
        public string Matchweek { get; set; }
        public string OtherClubName { get; set; }
        public string OtherClubCrest { get; set; }
        public string IsHomeClub { get; set; }
    }
}
