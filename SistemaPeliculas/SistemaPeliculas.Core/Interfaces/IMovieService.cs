using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task<Movie> InsertMovieAsync(Movie movie);
    }
}
