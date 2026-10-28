using System.Text.RegularExpressions;
using SistemaPeliculas.Core.CustomEntities;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Exceptions;
using SistemaPeliculas.Core.Interfaces;

namespace SistemaPeliculas.Core.Services
{
    public class ReviewService : IReviewService
    {
        // RN-10: palabras que no se permiten en reseñas y comentarios
        public static readonly string[] OffensiveWords =
        {
            "idiota", "estupido", "estúpido", "imbecil", "imbécil", "tarado",
            "mierda", "pendejo", "cojudo", "basura"
        };

        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;

        public ReviewService(IReviewRepository reviewRepository,
            IMovieRepository movieRepository, IUserRepository userRepository)
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllReviewsAsync();
        }

        public async Task<Review> GetReviewByIdAsync(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
                throw new BusinessException("Reseña no encontrada", 404);

            return review;
        }

        // CU-05 Consultar reseñas de una película
        public async Task<MovieReviewsSummary> GetReviewsByMovieAsync(int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null)
                throw new BusinessException("Película no encontrada", 404);

            var reviews = (await _reviewRepository.GetReviewsByMovieAsync(movieId)).ToList();

            return new MovieReviewsSummary
            {
                Movie = movie,
                Reviews = reviews,
                TotalReviews = reviews.Count,
                AverageGrade = reviews.Count == 0 ? null : Math.Round(reviews.Average(r => r.Grade), 1)
            };
        }

        public async Task<Review> InsertReviewAsync(Review review)
        {
            // RN-07: el usuario debe existir y estar activo
            var user = await _userRepository.GetUserByIdAsync(review.UserId ?? 0);
            if (user == null || !user.IsActive)
                throw new BusinessException("El usuario no existe o no está activo", 404);

            // RN-07: la película debe existir
            var movie = await _movieRepository.GetMovieByIdAsync(review.MovieId ?? 0);
            if (movie == null)
                throw new BusinessException("Película no encontrada", 404);

            // RN-08: solo se reseñan películas ya estrenadas
            if (movie.ReleaseDate.Date > DateTime.Today)
                throw new BusinessException("No se puede reseñar una película que todavía no se estrenó");

            // RN-09: calificación entre 1 y 10
            if (review.Grade < 1 || review.Grade > 10)
                throw new BusinessException("La calificación debe estar entre 1 y 10");

            // RN-10: sin palabras ofensivas
            review.Description = string.IsNullOrWhiteSpace(review.Description) ? null : review.Description.Trim();
            if (review.Description?.Length > 1000)
                throw new BusinessException("La reseña no puede tener más de 1000 caracteres");
            if (ContainsOffensiveWords(review.Description))
                throw new BusinessException("La reseña contiene palabras ofensivas");

            // RN-11: un usuario reseña una sola vez cada película
            if (await _reviewRepository.ExistsReviewAsync(user.Id, movie.Id))
                throw new BusinessException("El usuario ya publicó una reseña para esta película");

            review.Id = 0;            // lo genera la base de datos
            review.Date = DateTime.Now; // la fecha la pone el sistema

            await _reviewRepository.InsertReviewAsync(review);
            return review;
        }

        public static bool ContainsOffensiveWords(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            return OffensiveWords.Any(word =>
                Regex.IsMatch(text, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase));
        }
    }
}
