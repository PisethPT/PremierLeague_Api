namespace PremierLeague_Api.Dtos
{
    public class MatchesDto
    {
        public int MatchId { get; set; }
        public string MatchDate { get; set; }
        public string kickoffTime { get; set; }
        public string HomeClubName { get; set; }
        public string AwayClubName { get; set; }
        public string HomeClubCrest { get; set; }
        public string AwayClubCrest { get; set; }
        public string HomeClubTheme { get; set; }
        public string AwayClubTheme { get; set; }
        public string HomeClubGoal { get; set; }
        public string AwayClubGoal { get; set; }
        public string Competition { get; set; }
        public string KickoffStatus { get; set; }
        public string IsGameFinished { get; set; }
    }
}
