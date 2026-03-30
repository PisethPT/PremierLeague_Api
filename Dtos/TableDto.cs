namespace PremierLeague_Api.Dtos
{
    public class TableDto
    {
        public int Position { get; set; }
        public int ClubId { get; set; }
        public string ClubName { get; set; }
        public string ClubCrest { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GF { get; set; }
        public int GA { get; set; }
        public int GD { get; set; }
        public int Points { get; set; }
        public string Form { get; set; }
        public string Next { get; set; }
        public string Qualification { get; set; }
        public string PositionStatus { get; set; }
    }
}
