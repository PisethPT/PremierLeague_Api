namespace PremierLeague_Api.Dtos
{
    public class GroupedFavClubDto
    {
        public string MyClubLabel { get; set; } = string.Empty;
        public List<FavClubDto> Clubs { get; set; }

        public GroupedFavClubDto()
        {
            Clubs = new List<FavClubDto>();
        }
    }
}
