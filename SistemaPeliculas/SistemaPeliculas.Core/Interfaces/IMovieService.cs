using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.QueryFilters;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetMoviesAsync(MovieQueryFilter filter);
        Task<Movie> GetMovieByIdAsync(int id);
        Task<Movie> InsertMovieAsync(Movie movie);
    }
}
