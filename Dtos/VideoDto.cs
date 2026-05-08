namespace PremierLeague_Api.Dtos
{
    public class VideoDto
    {
        public int VideoId { get; set; }
        public string VideoLabel { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string ReferenceUrl { get; set; }
        public string VideoTag { get; set; }
        public string VideoUrl { get; set; }
        public bool IsReference { get; set; }
        public bool IsStory { get; set; }
        public bool IsVideoSeries { get; set; }
        public bool IsTheArchive { get; set; }
        public string Action { get; set; } = string.Empty;
        public string ButtonActionTitle { get; set; } = string.Empty;
    }
}
