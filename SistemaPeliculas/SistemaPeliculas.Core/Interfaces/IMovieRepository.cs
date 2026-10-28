using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<bool> ExistsTitleAsync(string title);
        Task InsertMovieAsync(Movie movie);
    }
}
