namespace PremierLeague_Api.Dtos
{
    public class NewsViewerDto
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }
        public string VideoUrl { get; set; }
        public string NewsTag { get; set; }
        public string PublishedDate { get; set; }
        public bool IsVideo { get; set; }
    }
}
