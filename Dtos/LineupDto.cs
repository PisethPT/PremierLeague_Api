namespace PremierLeague_Api.Dtos
{
    public class LineupDto
    {
        public int MatchId { get; set; }
        public int HomeClubFormationId { get; set; }
        public string HomeClubFormation { get; set; } = string.Empty;
        public string HomeClubShortName { get; set; } = string.Empty;
        public string HomeClubCrest { get; set; } = string.Empty;
        public string HomeClubManager { get; set; } = string.Empty;
        public string HomeClubTheme { get; set; } = string.Empty;
        public int AwayClubFormationId { get; set; }
        public string AwayClubFormation { get; set; } = string.Empty;
        public string AwayClubShortName { get; set; } = string.Empty;
        public string AwayClubCrest { get; set; } = string.Empty;
        public string AwayClubManager { get; set; } = string.Empty;
        public string AwayClubTheme { get; set; } = string.Empty;
        public List<ClubLineupDto> HomeClubLineups { get; set; }
        public List<ClubLineupDto> HomeClubSubstitutes { get; set; }
        public List<ClubLineupDto> AwayClubLineups { get; set; }
        public List<ClubLineupDto> AwayClubSubstitutes { get; set; }

        public LineupDto()
        {
            this.HomeClubLineups = new List<ClubLineupDto>();
            this.HomeClubSubstitutes = new List<ClubLineupDto>();
            this.AwayClubLineups = new List<ClubLineupDto>();
            this.HomeClubSubstitutes = new List<ClubLineupDto>();
        }
    }
}
