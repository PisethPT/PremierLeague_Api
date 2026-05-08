namespace PremierLeague_Api.Query
{
    public class NewsQuery
    {
        public int? NewsCategoryId { get; set; } = null;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
