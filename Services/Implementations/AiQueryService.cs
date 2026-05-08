using Microsoft.Extensions.Options;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using PremierLeague_Api.Dtos;
using PremierLeague_Api.Services.Interfaces;
using System.Text.Json;

namespace PremierLeague_Api.Services.Implementations
{
    public class AiQueryService : IAiQueryService
    {
        private readonly IOllamaApiClient _ollama;
        public AiQueryService()
        {
            _ollama = new OllamaApiClient("http://localhost:11434");
            _ollama.SelectedModel = "qwen2.5-coder:3b";
        }

        public async Task<AiResponseDto> GenerateSqlAsync(string userRequest)
        {
            var systemPrompt = @"You are a strict SQL Generator for a Premier League system using MS SQL Server (T-SQL).

### DATABASE SCHEMA:

-- CORE DATA --
- Teams: [Id, Name, ClubShortName, City, HomeStadium, HeadCoach, Founded, ClubCrest, TeamThemeColor]
- Players: [Id, FirstName, LastName, TeamId, Position, PlayerNumber, Nationality, DateOfBirth, Height]
- Seasons: [Id, Name, StartDate, EndDate, Competition, SeasonActive]
- Referees: [Id, FirstName, LastName, Nationality, DateOfBirth, IsActive]

-- MATCHES & LINEUPS --
- Matches: [Id, MatchDate, MatchTime, HomeTeamId, AwayTeamId, HomeTeamScore, AwayTeamScore, IsGameFinish]
- MatchLineups: [Id, MatchId, ClubId, PlayerId, IsStarting, FormationSlot]
- MatchSubstitutions: [Id, MatchId, ClubId, PlayerInId, PlayerOutId, Minute]
- MatchManOfTheMatches: [Id, MatchId, PlayerId, ClubId, AwardedBy]
- MatchReferees: [Id, MatchId, RefereeId, RoleId]
- MatchInfos: [Id, MatchId, Attendance, Weather, PitchCondition]

-- MATCH EVENTS --
- Goals: [Id, MatchId, PlayerId, TeamId, minutes]
- Assists: [Id, MatchId, PlayerId, TeamId, Minutes]
- Cards: [Id, MatchId, PlayerId, TeamId, Type, Minutes]
- MatchEvents: [Id, MatchId, PlayerId, ClubId, Minute, EventTypeId, OutcomeId, IsPenalty, IsOwnGoal, IsBigChance]

-- STATISTICS --
- PlayerStats: [Id, MatchId, PlayerId, ClubId, StatId, Value, PercentageValue]
- ClubStats: [Id, MatchId, ClubId, StatId, Value, PercentageValue]
- Stats: [Id, StatCode, StatName, StatCategoryId, IsPlayerStat, IsClubStat]

-- CONTENT & MEDIA --
- News: [Id, Title, SubTitle, Body, PublishedDate, ClubId, MatchId, IsFeatured]
- Videos: [Id, Title, Description, VideoUrl, ThumbnailUrl, PublishedDate, MatchId, PlayerId, ClubId]
- ClubSocialMedia: [Id, ClubId, SocialMediaTypeId, SocialMediaUrl]
- ClubServices: [Id, ClubId, Name, ServiceType, Url]

-- USER INTERACTION --
- FavoriteClubs: [Id, UserId, ClubId, FollowedDate]
- FavoritePlayers: [Id, UserId, PlayerId, FollowedDate]

### RELATIONSHIP GUIDES:
1. 'Teams.Id' links to 'Matches.HomeTeamId', 'Matches.AwayTeamId', 'Players.TeamId', and 'Goals.TeamId'.
2. 'Players.Id' links to 'Goals.PlayerId', 'Assists.PlayerId', 'MatchLineups.PlayerId', and 'PlayerStats.PlayerId'.
3. 'Matches.Id' links all match-related tables (Goals, Stats, Events, Lineups).

### CRITICAL T-SQL RULES:
1. NEVER use 'LIMIT'. Use 'SELECT TOP n' for limiting results.
2. Use 'Id' as the primary key for all tables.
3. For names/titles, use 'LIKE' with '%' (e.g., WHERE Name LIKE '%Arsenal%').
4. If asked for 'Scorers', join 'Players' and 'Goals'.
5. If asked for 'Top Stats', use 'ORDER BY Value DESC'.

### OUTPUT REQUIREMENT:
Return ONLY valid JSON. No prose. No markdown.
Format: { ""title"": """", ""sql"": """", ""description"": """", ""tip"": """" }";

            var request = new ChatRequest
            {
                Messages = new List<Message>
        {
            new Message(ChatRole.System, systemPrompt),
            new Message(ChatRole.User, userRequest)
        }
            };

            try
            {
                string fullResponse = "";
                await foreach (var res in _ollama.ChatAsync(request))
                {
                    fullResponse += res!.Message.Content;
                }

                string cleanJson = "";
                int firstBrace = fullResponse.IndexOf('{');
                int lastBrace = fullResponse.LastIndexOf('}');

                if (firstBrace == -1 || lastBrace == -1)
                    return GetFriendlyFallback("I couldn't structure that request correctly. Try asking about specific teams or matches.");

                cleanJson = fullResponse.Substring(firstBrace, (lastBrace - firstBrace) + 1)
                                        .Replace("```json", "").Replace("```", "").Trim();
       
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<AiResponseDto>(cleanJson, options);

                if (result != null && result.Sql.Contains("LIMIT", StringComparison.OrdinalIgnoreCase))
                {
                    return new AiResponseDto
                    {
                        Title = "Syntax Adjustment",
                        Sql = string.Empty, 
                        Description = "I generated a query using 'LIMIT', which isn't supported here. I'm adjusting my logic to use 'TOP' for your SQL Server.",
                        Tip = "Try asking: 'Show me the top 5 players'."
                    };
                }

                return result ?? GetFriendlyFallback();
            }
            catch (Exception)
            {
                return GetFriendlyFallback();
            }
        }

        private AiResponseDto GetFriendlyFallback(string? customMessage = null)
        {
            return new AiResponseDto
            {
                Title = "Assistant Note",
                Sql = string.Empty,
                Description = customMessage ?? "I ran into a small hiccup processing that request. Could you try rephrasing it?",
                Tip = "Pro Tip: Mention specific club names like 'Arsenal' or 'Liverpool' for better results!"
            };
        }

    }
}
