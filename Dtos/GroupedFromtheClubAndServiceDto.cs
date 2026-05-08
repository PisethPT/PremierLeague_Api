namespace PremierLeague_Api.Dtos
{
    public class GroupedFromtheClubAndServiceDto
    {
        public List<FromTheClubNewsDto> FromTheClubs { get; set; }
        public List<ClubServiceDto> ClubServices { get; set; }

        public GroupedFromtheClubAndServiceDto()
        {
            this.FromTheClubs = new List<FromTheClubNewsDto>();
            this.ClubServices = new List<ClubServiceDto>();
        }
    }
}
