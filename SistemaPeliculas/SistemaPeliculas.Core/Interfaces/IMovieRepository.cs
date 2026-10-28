using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.QueryFilters;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetMoviesAsync(MovieQueryFilter filter);
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<bool> ExistsTitleAsync(string title);
        Task InsertMovieAsync(Movie movie);
    }
}
