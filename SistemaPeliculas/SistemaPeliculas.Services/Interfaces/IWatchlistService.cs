using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Services.Interfaces
{
    public interface IWatchlistService
    {
        Task<Watchlist> AddToWatchlistAsync(int userId, int movieId);
        Task<IEnumerable<Watchlist>> GetWatchlistByUserIdAsync(int userId);
        Task DeleteWatchlistAsync(int id);
    }
}

