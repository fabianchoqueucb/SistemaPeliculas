using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Core.QueryFilters;
using SistemaPeliculas.Infrastructure.Data;

namespace SistemaPeliculas.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly PeliculasContext _context;

        public MovieRepository(PeliculasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetMoviesAsync(MovieQueryFilter filter)
        {
            // IQueryable: la consulta se arma por partes y recién se ejecuta en ToListAsync()
            IQueryable<Movie> query = _context.Movies;

            if (!string.IsNullOrWhiteSpace(filter.Title))
                query = query.Where(m => m.Title.Contains(filter.Title));

            if (!string.IsNullOrWhiteSpace(filter.Genre))
                query = query.Where(m => m.Genre == filter.Genre);

            if (filter.Year.HasValue)
                query = query.Where(m => m.ReleaseDate.Year == filter.Year.Value);

            return await query.OrderBy(m => m.Title).ToListAsync();
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> ExistsTitleAsync(string title)
        {
            return await _context.Movies.AnyAsync(m => m.Title.ToLower() == title.ToLower());
        }

        public async Task InsertMovieAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
        }
    }
}
