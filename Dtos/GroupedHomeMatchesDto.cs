namespace PremierLeague_Api.Dtos
{
    public class GroupedHomeMatchesDto
    {
        public string MatchDate { get; set; }
        public IEnumerable<HomeMatchesDto> Matches { get; set; }
    }
}
