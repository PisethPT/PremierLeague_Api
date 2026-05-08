namespace PremierLeague_Api.Dtos
{
    public class MatchInfoDto
    {
        public int MatchId { get; set; }
        public int HomeClubId { get; set; }
        public int AwayClubId { get; set; }
        public string MatchDate { get; set; }
        public string KickoffTime { get; set; }
        public string HomeClubName { get; set; }
        public string HomeClubCrest { get; set; }
        public string HomeClubTheme { get; set; }
        public int HomeClubGoal { get; set; }
        public string AwayClubName { get; set; }
        public string AwayClubCrest { get; set; }
        public string AwayClubTheme { get; set; }
        public int AwayClubGoal { get; set; }
        public string Competition { get; set; }
        public string Stadium { get; set; }
        public string Referee { get; set; }
        public string Matchweek { get; set; }
        public string MatchInfo { get; set; }
        public string KickoffStatusDisplay { get; set; }
        public string IsGameFinished { get; set; }
    }
}
