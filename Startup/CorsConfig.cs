namespace PremierLeague_Api.Startup
{
    public static class CorsConfig
    {
        private const string CORS_POLICY_NAME = "PREMIER_LEAGUE-X-POLICY";
        private const string CLIENT_BASE_URL = "http://localhost:5173";
        public static void AddCorsPolicyServices(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY_NAME, policy =>
                {
                    policy.WithOrigins(CLIENT_BASE_URL)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }

        public static void UseCorsPolicyServices(this WebApplication app)
        {
            app.UseCors(CORS_POLICY_NAME);
        }
    }
}
