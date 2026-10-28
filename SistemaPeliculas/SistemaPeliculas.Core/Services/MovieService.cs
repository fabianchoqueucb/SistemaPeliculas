using System.Text.RegularExpressions;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Exceptions;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Core.QueryFilters;

namespace SistemaPeliculas.Core.Services
{
    public class MovieService : IMovieService
    {
        // RN-04: lista de géneros válidos
        public static readonly string[] ValidGenres =
        {
            "Acción", "Animación", "Aventura", "Ciencia ficción", "Comedia", "Documental",
            "Drama", "Fantasía", "Musical", "Romance", "Suspenso", "Terror"
        };

        // RN-05: duración con formato "#h #m", por ejemplo "2h 15m"
        private static readonly Regex LengthFormat = new(@"^(\d{1,2})h\s?(\d{1,2})m$");

        // RN-06: no puede ser anterior a la primera proyección pública del cine (28/12/1895)
        private static readonly DateTime MinReleaseDate = new(1895, 12, 28);

        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        // CU-04 Consultar catálogo de películas
        public async Task<IEnumerable<Movie>> GetMoviesAsync(MovieQueryFilter filter)
        {
            filter.Title = filter.Title?.Trim();

            // Si filtran por género, debe ser uno de la lista (RN-04)
            if (!string.IsNullOrWhiteSpace(filter.Genre))
            {
                var genre = ValidGenres.FirstOrDefault(g =>
                    string.Equals(g, filter.Genre.Trim(), StringComparison.OrdinalIgnoreCase));
                if (genre == null)
                    throw new BusinessException(
                        $"Género no válido. Géneros permitidos: {string.Join(", ", ValidGenres)}");
                filter.Genre = genre;
            }

            if (filter.Year.HasValue && (filter.Year < MinReleaseDate.Year || filter.Year > DateTime.Today.Year + 5))
                throw new BusinessException($"El año debe estar entre {MinReleaseDate.Year} y {DateTime.Today.Year + 5}");

            return await _movieRepository.GetMoviesAsync(filter);
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
                throw new BusinessException("Película no encontrada", 404);

            return movie;
        }

        public async Task<Movie> InsertMovieAsync(Movie movie)
        {
            // RN-01: título y descripción obligatorios
            movie.Title = movie.Title?.Trim() ?? "";
            movie.Description = movie.Description?.Trim() ?? "";

            if (movie.Title == "")
                throw new BusinessException("El título es obligatorio");
            if (movie.Description == "")
                throw new BusinessException("La descripción es obligatoria");

            // RN-02: longitud máxima de título y descripción
            if (movie.Title.Length > 100)
                throw new BusinessException("El título no puede tener más de 100 caracteres");
            if (movie.Description.Length > 1000)
                throw new BusinessException("La descripción no puede tener más de 1000 caracteres");

            // RN-03: no se repite el título
            if (await _movieRepository.ExistsTitleAsync(movie.Title))
                throw new BusinessException($"Ya existe una película con el título '{movie.Title}'");

            // RN-04: género dentro de la lista
            var genre = ValidGenres.FirstOrDefault(g =>
                string.Equals(g, movie.Genre?.Trim(), StringComparison.OrdinalIgnoreCase));
            if (genre == null)
                throw new BusinessException(
                    $"Género no válido. Géneros permitidos: {string.Join(", ", ValidGenres)}");
            movie.Genre = genre;

            // RN-05: duración entre 1h y 4h con formato "#h #m"
            var match = LengthFormat.Match(movie.Length?.Trim() ?? "");
            if (!match.Success || int.Parse(match.Groups[2].Value) > 59)
                throw new BusinessException("La duración debe tener el formato '#h #m', por ejemplo '2h 15m'");

            var totalMinutes = int.Parse(match.Groups[1].Value) * 60 + int.Parse(match.Groups[2].Value);
            if (totalMinutes < 60 || totalMinutes > 240)
                throw new BusinessException("La duración debe estar entre 1h 0m y 4h 0m");
            movie.Length = $"{match.Groups[1].Value}h {match.Groups[2].Value}m";

            // RN-06: fecha de estreno válida
            if (movie.ReleaseDate.Date < MinReleaseDate)
                throw new BusinessException("La fecha de estreno no puede ser anterior al 28/12/1895");

            movie.Id = 0; // el Id lo genera la base de datos
            movie.ReleaseDate = movie.ReleaseDate.Date;

            await _movieRepository.InsertMovieAsync(movie);
            return movie;
        }
    }
}
