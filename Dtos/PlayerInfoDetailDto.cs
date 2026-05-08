namespace PremierLeague_Api.Dtos
{
    public class PlayerInfoDetailDto
    {
        public int PlayerId { get; set; }
        public int ClubId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PlayerNumber { get; set; }
        public string Position { get; set; }
        public string Photo { get; set; }
        public string Nationality { get; set; }
        public string PlaceOfBirth { get; set; }
        public string PreferredFoot { get; set; }
        public string DateOfBirth { get; set; }
        public string Height { get; set; }
        public string JoinedClub { get; set; }
        public string ClubName { get; set; }
        public string ClubShortName { get; set; }
        public string ClubCrest { get; set; }
        public string ClubTheme { get; set; }
        public int Appearances { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
    }
}
