namespace PremierLeague_Api.Dtos
{
    public class GroupedmyPLSettingsDto
    {
        public List<myPLSettingsDto> Info { get; set; }
        public List<MatchesDto> Matches { get; set; }
        public List<myPLSettings_FollowingClubDto> FollowingClubs { get; set; }
        public List<myPLSettings_FollowingPlayerDto> FollowingPlayers { get; set; }

        public GroupedmyPLSettingsDto()
        {
            Info = new List<myPLSettingsDto>();
            Matches = new List<MatchesDto>();
            FollowingClubs = new List<myPLSettings_FollowingClubDto>();
            FollowingPlayers = new List<myPLSettings_FollowingPlayerDto>();
        }
    }
}
