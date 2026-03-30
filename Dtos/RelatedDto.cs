namespace PremierLeague_Api.Dtos
{
    public class RelatedDto
    {
        public int RelatedId { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string ReferenceUrl { get; set; }
        public string TagName { get; set; }
        public bool IsVideo { get; set; }
    }
}
