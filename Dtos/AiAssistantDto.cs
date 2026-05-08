using Microsoft.Extensions.AI;

namespace PremierLeague_Api.Dtos
{
    public class AiAssistantDto
    {
        public AiResponseDto Chat { get; set; }
        public dynamic Data { get; set; }
    }
}
