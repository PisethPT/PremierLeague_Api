namespace PremierLeague_Api.Dtos
{
    public class GroupedMatchesDto
    {
        public string MatchDate { get; set; }
        public IEnumerable<HomeMatchesDto> Matches { get; set; }
    }
}
