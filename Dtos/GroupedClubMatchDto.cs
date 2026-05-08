namespace PremierLeague_Api.Dtos
{
    public class GroupedClubMatchDto
    {
        public List<MonthDto> Months { get; set; }
        public List<MatchesDto> Matches { get; set; }

        public GroupedClubMatchDto()
        {
            this.Months = new List<MonthDto>();
            this.Matches = new List<MatchesDto>();
        }
    }
}
