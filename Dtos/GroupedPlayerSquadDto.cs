namespace PremierLeague_Api.Dtos
{
    public class GroupedPlayerSquadDto
    {
        public string PositionLabel { get; set; } = string.Empty;
        public List<PlayerDto> Players { get; set; }

        public GroupedPlayerSquadDto()
        {
            this.Players = new List<PlayerDto>();
        }
    }
}
