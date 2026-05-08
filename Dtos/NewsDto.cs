namespace PremierLeague_Api.Dtos
{
    public class NewsDto
    {
        public string NewsLabel { get; set; }
        public int TopicId { get; set; }
        public string Title { get; set; }
        public string TopicTag { get; set; }
        public string Thumbnail { get; set; }
        public string ReferenceUrl { get; set; }
        public string VideoUrl { get; set; }
        public string ButtonActionTitle { get; set; }
        public string Action { get; set; }
        public bool IsVideo { get; set; }
        public bool IsArrows { get; set; }
        public bool IsViewMore { get; set; }
        public int TotalNews { get; set; }
        public int CategoryId { get; set; }
    }
}
