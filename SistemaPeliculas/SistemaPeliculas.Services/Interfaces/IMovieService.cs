using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Services.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync(string? title = null, string? genre = null);
        Task<Movie?> GetMovieByIdAsync(int id);
        Task InsertMovie(Movie movie);
        Task UpdateMovie(Movie movie);
        Task DeleteMovie(int id);
    }
}

