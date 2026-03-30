namespace PremierLeague_Api.Dtos
{
    public class GroupedVideoDto
    {
        public string VideoLabel { get; set; }
        public IEnumerable<HomeVideoDto> Videos { get; set; }
    }
}
