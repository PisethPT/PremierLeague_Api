namespace PremierLeague_Api.Dtos
{
    public class MatchInfoDetailDto
    {
        public MatchInfoDto MatchInfo { get; set; }

        public MatchInfoDetailDto()
        {
            MatchInfo = new MatchInfoDto();
        }
    }
}
