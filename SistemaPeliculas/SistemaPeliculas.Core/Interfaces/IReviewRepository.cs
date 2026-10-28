using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review?> GetReviewByIdAsync(int id);
        Task<bool> ExistsReviewAsync(int userId, int movieId);
        Task InsertReviewAsync(Review review);
    }
}
