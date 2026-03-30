namespace PremierLeague_Api.Dtos
{
    public class VideoRelatedGroupDto
    {
        public VideoViewerDto Video { get; set; }
        public List<RelatedDto> Relateds { get; set; }

        public VideoRelatedGroupDto()
        {
            Video = new VideoViewerDto();
            Relateds = new List<RelatedDto>();
        }
    }
}
