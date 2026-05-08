namespace PremierLeague_Api.Dtos
{
    public class TeamFormPreviousDto
    {
        public int MatchId { get; set; }
        public string HomeClubName { get; set; }
        public string AwayClubName { get; set; }
        public string MatchDate { get; set; }
        public string Matchweek { get; set; }
        public int HomeClubGoal { get; set; }
        public string OtherClubName { get; set; }
        public string OtherClubCrest { get; set; }
        public int OtherClubGoal { get; set; }
        public string IsHomeClub { get; set; }
        public string MatchResult { get; set; }
    }
}
