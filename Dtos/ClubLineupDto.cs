namespace PremierLeague_Api.Dtos
{
    public class ClubLineupDto
    {
        public int ClubId { get; set; }
        public int PlayerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PlayerPhoto { get; set; }
        public int PlayerNumber { get; set; }
        public int FormationPositionId { get; set; }
        public bool IsStarting { get; set; }
        public int FormationSlot { get; set; }
        public string InMinute { get; set; } = string.Empty;
        public string OutMinute { get; set; } = string.Empty;
        public int Goals { get; set; }
        public int Assists { get; set; }
        public string HasCard { get; set; } // yellow or read card
        public bool IsHasGoal { get; set; }
        public bool IsHasAssist { get; set; }
        public bool IsCaption { get; set; }

    }
}
