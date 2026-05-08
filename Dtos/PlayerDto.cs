namespace PremierLeague_Api.Dtos
{
    public class PlayerDto
    {
        public int PlayerId { get; set; }
        public int ClubId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerPhoto { get; set; }
        public string Position { get; set; }
        public int PlayerNumber { get; set; }
        public string Nationality { get; set; }
        public string ClubName { get; set; }
        public string ClubCrest { get; set; }
        public string ClubTheme { get; set; }
        public string PositionLabel { get; set; }
    }
}
