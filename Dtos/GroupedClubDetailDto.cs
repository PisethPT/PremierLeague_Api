namespace PremierLeague_Api.Dtos
{
    public class GroupedClubDetailDto
    {
        public ClubDetailDto ClubDetail { get; set; }
        public List<ClubSocialMediaDto> ClubSocialMedias { get; set; }

        public List<StoryNewsDto> StoryNews { get; set; }

        public GroupedClubDetailDto()
        {
            this.ClubDetail = new ClubDetailDto();
            this.ClubSocialMedias = new List<ClubSocialMediaDto>();
            this.StoryNews = new List<StoryNewsDto>();
        }
    }
}
