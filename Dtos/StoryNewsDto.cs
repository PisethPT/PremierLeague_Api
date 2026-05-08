namespace PremierLeague_Api.Dtos
{
    public class StoryNewsDto
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string VideoTag { get; set; }
        public string Thumbnail { get; set; }
        public string VideoUrl { get; set; }
        public decimal Duration { get; set; }
    }
}
