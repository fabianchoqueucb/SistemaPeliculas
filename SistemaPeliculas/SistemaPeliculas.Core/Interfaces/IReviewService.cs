using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task<Review> InsertReviewAsync(Review review);
    }
}
