namespace PremierLeague_Api.Dtos
{
    public class GroupedMatchDto
    {
        public int Matchweek { get; set; }
        public string MatchDate { get; set; }
        public string LatestMatchWeek { get; set; }
        public string LatestMatchweekDateRange { get; set; }
        public IEnumerable<MatchDto> Matches { get; set; }

        public GroupedMatchDto()
        {
            Matches = new List<MatchDto>();
        }
    }
}
