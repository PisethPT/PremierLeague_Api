namespace PremierLeague_Api.Dtos
{
    public class ClubNewsDto
    {
        public int NewsId { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string ReferenceUrl { get; set; }
        public string TagName { get; set; }
        public string ClubCrest { get; set; }
        public string ClubTheme { get; set; }
    }
}
