using PremierLeague_Api.Dtos;
using PremierLeague_Api.Responses;

namespace PremierLeague_Api.Repositories.Interfaces
{
    public interface IClubRepository
    {
        Task<Response<IEnumerable<ClubDto>>> GetSeasonClubsAsync(int? currentSeasonId = null, CancellationToken ct = default);
        Task<Response<IEnumerable<TableDto>>> GetClubTableAsync(int? seasonId = 4, int? clubId = null, CancellationToken ct = default);
        Task<Response<IEnumerable<ClubDto>>> GetAllTimePremierLeagueClubsAsync(CancellationToken ct = default);
        Task<Response<GroupedClubDetailDto>> GetClubDetailAsync(int clubId, CancellationToken ct = default);
        Task<Response<GroupedClubOverviewNextMatchAndTeamFormDto>> GetClubOverviewNextMatchAndTeamFormAsync(int clubId, CancellationToken ct = default);
        Task<Response<ClubLastStarting11Dto>> GetClubLastStarting11Async(int clubId, CancellationToken ct = default);
        Task<Response<IEnumerable<PrimaryFormationDto>>> GetFormationAsync(CancellationToken ct = default);
        Task<Response<GroupedFromtheClubAndServiceDto>> GetClubNewsAndServicesAsync(int clubId, CancellationToken ct = default);
        Task<Response<IEnumerable<GroupedPlayerSquadDto>>> GetClubSquadAsync(int seasonId = 4, int? clubId = null, CancellationToken ct = default);
        Task<Response<IEnumerable<MatchesDto>>> GetClubMatchesAsync(int seasonId = 4, int? clubId = null, int? month = null, CancellationToken ct = default);
        Task<Response<IEnumerable<MonthDto>>> GetClubMonthOfMatchAsync(int? seasonId = 4, int? clubId = null, CancellationToken ct = default);
        Task<Response<IEnumerable<ClubNewsDto>>> GetClubNewsAsync(int? seasonId = 4, int? clubId = null, CancellationToken ct = default);
        Task<Response<IEnumerable<HomeVideoDto>>> GetClubVideosAsync(int? seasonId = 4, int? clubId = null, CancellationToken ct = default);
    }
}
