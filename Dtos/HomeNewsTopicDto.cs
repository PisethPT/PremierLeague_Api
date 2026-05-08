namespace PremierLeague_Api.Dtos
{
    public class HomeNewsTopicDto
    {
        public int TopicId { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string TopicTag { get; set; }
        public string VideoUrl { get; set; }
        public string ReferenceUrl { get; set; }
        public bool IsVideo { get; set; }
    }
}
