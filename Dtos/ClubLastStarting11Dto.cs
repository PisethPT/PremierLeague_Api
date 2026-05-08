namespace PremierLeague_Api.Dtos
{
    public class ClubLastStarting11Dto
    {
        public int MatchId { get; set; }
        public int ClubFormationId { get; set; }
        public string ClubManager { get; set; } = string.Empty;
        public LastMatchDto LastMatch { get; set; }
        public List<ClubLineupDto> ClubLineups { get; set; }

        public ClubLastStarting11Dto()
        {
            this.LastMatch = new LastMatchDto();
            this.ClubLineups = new List<ClubLineupDto>();
        }
    }
}
