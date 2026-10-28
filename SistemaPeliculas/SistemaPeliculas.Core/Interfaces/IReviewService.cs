using SistemaPeliculas.Core.CustomEntities;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int id);
        Task<MovieReviewsSummary> GetReviewsByMovieAsync(int movieId);
        Task<Review> InsertReviewAsync(Review review);
    }
}
