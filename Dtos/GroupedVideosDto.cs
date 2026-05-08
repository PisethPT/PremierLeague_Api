namespace PremierLeague_Api.Dtos
{
    public class GroupedVideosDto
    {
        public string VideoLabel { get; set; } = string.Empty;
        public bool IsStory { get; set; }
        public bool IsVideoSeries { get; set; }
        public bool IsTheArchive { get; set; }
        public string Action { get; set; } = string.Empty;
        public string ButtonActionTitle { get; set; } = string.Empty;
        public IEnumerable<VideoDto> Videos { get; set; }

        public GroupedVideosDto()
        {
            this.Videos = new List<VideoDto>();
        }
    }
}
