namespace PremierLeague_Api.Dtos
{
    public class GroupedMatchInfoDto
    {
        public MatchDetailDto MatchDetail { get; set; }
        public List<MatchOfficialDto> MatchOfficials { get; set; }

        public GroupedMatchInfoDto()
        {
            this.MatchDetail = new MatchDetailDto();
            this.MatchOfficials = new List<MatchOfficialDto>();
        }
    }
}
