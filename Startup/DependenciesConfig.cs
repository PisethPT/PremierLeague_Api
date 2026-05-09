using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OllamaSharp;
using PremierLeague_Api.Repositories.Implementations;
using PremierLeague_Api.Repositories.Interfaces;
using PremierLeague_Api.Services.Implementations;
using PremierLeague_Api.Services.Interfaces;
using System.Text;

namespace PremierLeague_Api.Startup
{
    public static class DependenciesConfig
    {
        // Jwt Service
        public static void ReisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });


            builder.Services.AddControllers();

            builder.Services.AddCorsPolicyServices();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthentication();

            // Redis Cache configuration
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddDistributedMemoryCache();
            }
            else
            {
                // Docker / Production
                builder.Services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration =
                        builder.Configuration.GetConnectionString("RedisConnection");
                    options.InstanceName = "PremierLeagueApi:";
                });
            }

            builder.Services.AddScoped<IOllamaApiClient>(sp =>
            {
                // Use a custom HttpClient to control the timeout
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:11434"),
                    Timeout = TimeSpan.FromSeconds(100) // Increased from default 20s
                };

                var client = new OllamaApiClient(httpClient);
                client.SelectedModel = "qwen2.5-coder:3b";
                return client;
            });

            builder.Services.AddScoped<ICacheService, RedisCacheService>();

            // Repositories
            builder.Services.AddScoped<IExecuteQuery, ExecuteQuery>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IHomeRepository, HomeRepository>();
            builder.Services.AddScoped<IMatchRepository, MatchRepository>();
            builder.Services.AddScoped<ITableRepository, TableRepository>();
            builder.Services.AddScoped<IClubRepository, ClubRepository>();
            builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
            builder.Services.AddScoped<IVideoRepository, VideoRepository>();
            builder.Services.AddScoped<INewsRepository, NewsRepository>();

            builder.Services.AddScoped<IAiQueryService, AiQueryService>();
            builder.Services.AddScoped<IAiRepository, AiRepository>();


            builder.Services.AddScoped<ISelectListItemRepository, SelectListItemRepository>();

            builder.Services.AddOpenApi();
        }
    }
}
