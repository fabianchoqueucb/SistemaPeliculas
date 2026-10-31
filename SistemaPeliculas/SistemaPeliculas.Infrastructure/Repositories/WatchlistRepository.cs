using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Infrastructure.Data;

namespace SistemaPeliculas.Infrastructure.Repositories
{
    public class WatchlistRepository : BaseRepository<Watchlist>, IWatchlistRepository
    {
        public WatchlistRepository(PeliculasContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Watchlist>> GetByUserIdAsync(int userId)
        {
            return await _context.Watchlists
                .Include(w => w.Movie)
                .Include(w => w.User)
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int userId, int movieId)
        {
            return await _context.Watchlists
                .AnyAsync(w => w.UserId == userId && w.MovieId == movieId);
        }

        public async Task<Watchlist?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Watchlists
                .Include(w => w.Movie)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}

