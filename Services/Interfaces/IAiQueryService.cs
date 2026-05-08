using PremierLeague_Api.Dtos;

namespace PremierLeague_Api.Services.Interfaces
{
    public interface IAiQueryService
    {
        Task<AiResponseDto> GenerateSqlAsync(string userRequest);
    }
}
