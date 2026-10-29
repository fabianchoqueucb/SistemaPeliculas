using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(int id);
        Task<IEnumerable<Review>> GetReviewsByMovieAsync(int movieId);
        Task InsertReview(Review review);
        Task UpdateReview(Review review);
        Task DeleteReview(Review review);
        Task<bool> ExistsReviewAsync(int userId, int movieId);
    }
}
