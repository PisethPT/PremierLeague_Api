namespace PremierLeague_Api.Query
{
    public class FavRequest
    {
        public string? Email { get; set; }
        public List<int> JsonData { get; set; } = new();
    }
}
