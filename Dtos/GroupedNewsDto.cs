namespace PremierLeague_Api.Dtos
{
    public class GroupedNewsDto
    {
        public string NewsLabel { get; set; }
        public IEnumerable<HomeNewsDto> News { get; set; }
    }
}
