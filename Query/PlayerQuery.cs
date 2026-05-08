namespace PremierLeague_Api.Query
{
    public class PlayerQuery
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? Competition { get; set; }
        public int? Season { get; set; }
        public List<int> Clubs { get; set; } = new();
        public List<int> Positions { get; set; } = new();
    }
}
