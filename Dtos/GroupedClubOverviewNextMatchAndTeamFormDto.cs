namespace PremierLeague_Api.Dtos
{
    public class GroupedClubOverviewNextMatchAndTeamFormDto
    {
        public MatchesDto NextMatch { get; set; }
        public ClubDetailDto ClubDetail { get; set; }
        public List<TeamFormPreviousDto> TeamFormPrevious { get; set; }
        public List<TeamFormUpcomingDto> TeamFormUpcoming { get; set; }

        public GroupedClubOverviewNextMatchAndTeamFormDto()
        {
            this.NextMatch = new MatchesDto();
            this.ClubDetail= new ClubDetailDto();
            this.TeamFormPrevious = new List<TeamFormPreviousDto>();
            this.TeamFormUpcoming = new List<TeamFormUpcomingDto>();
        }
    }
}
