namespace PremierLeague_Api.Dtos
{
    public class SiginGoogleDto
    {
        /// <summary>
        /// The Primary Key from the AspNetUsers table (Id).
        /// Populate this after the database check.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// The unique ID provided by Google (sub claim).
        /// </summary>
        public string GoogleId { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;
        public string? Locale { get; set; }
        public string? Gender { get; set; }

        public string? PhotoUrl { get; set; }
    }
}