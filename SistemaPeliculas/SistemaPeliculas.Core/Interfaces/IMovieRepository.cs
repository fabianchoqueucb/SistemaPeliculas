using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync(string? title = null, string? genre = null);
        Task<Movie?> GetMovieByIdAsync(int id);
        Task InsertMovie(Movie movie);
        Task UpdateMovie(Movie movie);
        Task DeleteMovie(Movie movie);
        Task<bool> ExistsTitleAsync(string title);
    }
}
