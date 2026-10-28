using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;
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

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
        {
            return await _context.Movies.ToListAsync();
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
