namespace PremierLeague_Api.Dtos
{
    public class MatchDto
    {
        public int MatchId { get; set; }
        public string MatchDate { get; set; }
        public int Matchweek { get; set; }
        public string LatestMatchWeek { get; set; }
        public string LatestMatchweekDateRange { get; set; }
        public string MatchTime { get; set; }
        public int HomeClubId { get; set; }
        public string HomeClubName { get; set; }
        public string HomeClubCrest { get; set; }
        public int HomeClubGoal { get; set; }
        public int AwayClubId { get; set; }
        public string AwayClubName { get; set; }
        public string AwayClubCrest { get; set; }
        public int AwayClubGoal { get; set; }
        public string KickoffStatus { get; set; }
        public string IsGameFinished { get; set; }
    }
}
