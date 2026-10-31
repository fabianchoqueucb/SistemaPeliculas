using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IWatchlistRepository : IBaseRepository<Watchlist>
    {
        Task<IEnumerable<Watchlist>> GetByUserIdAsync(int userId);
        Task<bool> ExistsAsync(int userId, int movieId);
        Task<Watchlist?> GetByIdWithDetailsAsync(int id);
    }
}

