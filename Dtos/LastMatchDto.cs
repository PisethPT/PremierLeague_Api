namespace PremierLeague_Api.Dtos
{
    public class LastMatchDto
    {
        public int MatchId { get; set; }
        public string MatchDate { get; set; }
        public string Matchweek { get; set; }
        public string HomeClubName { get; set; }
        public string AwayClubName { get; set; }
        public string HomeClubCrest { get; set; }
        public string AwayClubCrest { get; set; }
        public int HomeClubGoal { get; set; }
        public int AwayClubGoal { get; set; }
    }
}
