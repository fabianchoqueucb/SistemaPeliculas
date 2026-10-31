using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Exceptions;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Services.Interfaces;

namespace SistemaPeliculas.Services.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesAsync(string? title = null, string? genre = null)
        {
            return await _movieRepository.GetAllMoviesAsync(title, genre);
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _movieRepository.GetMovieByIdAsync(id);
        }

        public async Task InsertMovie(Movie movie)
        {
            // 1. Validar que no se repita el título
            if (await _movieRepository.ExistsTitleAsync(movie.Title))
                throw new BusinessException($"Ya existe una película con el título '{movie.Title}'");

            // 2. Validar que la fecha de estreno no sea anterior a los inicios del cine (1895)
            if (movie.ReleaseDate < new DateTime(1895, 12, 28))
                throw new BusinessException("La fecha de estreno no puede ser anterior al 28/12/1895");

            // 3. No permitir contenido o palabras prohibidas en la descripción
            if (ContainsForbiddenContent(movie.Description))
                throw new BusinessException("La descripción contiene palabras no permitidas");

            await _movieRepository.InsertMovie(movie);
        }

        public async Task UpdateMovie(Movie movie)
        {
            await _movieRepository.UpdateMovie(movie);
        }

        public async Task DeleteMovie(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
                throw new BusinessException("La película no existe");

            await _movieRepository.DeleteMovie(movie);
        }

        public readonly string[] ForbiddenWords =
        {
            "violencia",
            "odio",
            "grosería",
            "discriminación",
            "piratería"
        };

        public bool ContainsForbiddenContent(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            foreach (var word in ForbiddenWords)
            {
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }
    }
}
