using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Infrastructure.Data;

namespace SistemaPeliculas.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly PeliculasContext _context;

        public ReviewRepository(PeliculasContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> ExistsReviewAsync(int userId, int movieId)
        {
            return await _context.Reviews.AnyAsync(r => r.UserId == userId && r.MovieId == movieId);
        }

        public async Task InsertReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }
    }
}
