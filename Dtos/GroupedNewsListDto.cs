namespace PremierLeague_Api.Dtos
{
    public class GroupedNewsListDto
    {
        public string NewsLabel { get; set; } = string.Empty;
        public bool IsVideo { get; set; }
        public bool IsArrows { get; set; }
        public bool IsViewMore { get; set; }
        public string Action { get; set; } = string.Empty;
        public string ButtonActionTitle { get; set; } = string.Empty;
        public int TotalNews { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<NewsDto> News { get; set; }

        public GroupedNewsListDto()
        {
            News = new List<NewsDto>();
        }
    }
}
