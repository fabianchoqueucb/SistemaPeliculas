using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Services.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByMovieAsync(int movieId);
        Task InsertReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(int id);
    }
}

