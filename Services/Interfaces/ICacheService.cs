namespace PremierLeague_Api.Services.Interfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T value, int minutes);

        Task RemoveAsync(string key);

        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getData, int minutes);
    }
}
