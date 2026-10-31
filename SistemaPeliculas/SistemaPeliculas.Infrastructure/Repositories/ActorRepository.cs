using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Infrastructure.Data;

namespace SistemaPeliculas.Infrastructure.Repositories
{
    public class ActorRepository : BaseRepository<Actor>, IActorRepository
    {
        public ActorRepository(PeliculasContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Actor>> GetActorsAsync(string? name = null, int? movieId = null)
        {
            var query = _context.Actors.Include(a => a.Movie).AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(a => a.FirstName.Contains(name) || a.LastName.Contains(name));
            }

            if (movieId.HasValue && movieId.Value > 0)
            {
                query = query.Where(a => a.MovieId == movieId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<Actor?> GetActorWithDetailsAsync(int id)
        {
            return await _context.Actors
                .Include(a => a.Movie)
                .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}

