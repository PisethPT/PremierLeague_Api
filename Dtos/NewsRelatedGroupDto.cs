namespace PremierLeague_Api.Dtos
{
    public class NewsRelatedGroupDto
    {
        public NewsViewerDto News { get; set; }
        public List<RelatedDto> Relateds { get; set; }

        public NewsRelatedGroupDto()
        {
            News = new NewsViewerDto();
            Relateds = new List<RelatedDto>();
        }
    }
}
