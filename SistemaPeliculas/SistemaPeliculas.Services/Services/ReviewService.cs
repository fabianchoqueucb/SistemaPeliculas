using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Services.Interfaces;

namespace SistemaPeliculas.Services.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;

        public ReviewService(IReviewRepository reviewRepository,
            IMovieRepository movieRepository,
            IUserRepository userRepository)
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _reviewRepository.GetAllReviewsAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await _reviewRepository.GetReviewByIdAsync(id);
        }

        public async Task<IEnumerable<Review>> GetReviewsByMovieAsync(int movieId)
        {
            return await _reviewRepository.GetReviewsByMovieAsync(movieId);
        }

        public async Task InsertReview(Review review)
        {
            // 1. Validar que el usuario exista
            var user = await _userRepository.GetUserByIdAsync(review.UserId ?? 0);
            if (user == null)
                throw new Exception("El usuario no existe");

            // 2. Validar que la película exista
            var movie = await _movieRepository.GetMovieByIdAsync(review.MovieId ?? 0);
            if (movie == null)
                throw new Exception("La película no existe");

            // 3. Validar que la película ya haya sido estrenada
            if (movie.ReleaseDate > DateTime.Today)
                throw new Exception("No se puede reseñar una película que todavía no se estrenó");

            // 4. Validar calificación de 1 a 10
            if (review.Grade < 1 || review.Grade > 10)
                throw new Exception("La calificación debe estar entre 1 y 10");

            // 5. Validar que no contenga palabras inapropiadas
            if (ContainsForbiddenContent(review.Description))
                throw new Exception("La reseña contiene palabras no permitidas");

            // 6. Un usuario no puede reseñar dos veces la misma película
            if (await _reviewRepository.ExistsReviewAsync(review.UserId ?? 0, review.MovieId ?? 0))
                throw new Exception("El usuario ya publicó una reseña para esta película");

            review.Date = DateTime.Now;
            await _reviewRepository.InsertReview(review);
        }

        public async Task UpdateReview(Review review)
        {
            await _reviewRepository.UpdateReview(review);
        }

        public async Task DeleteReview(int id)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(id);
            if (review == null)
                throw new Exception("La reseña no existe");

            await _reviewRepository.DeleteReview(review);
        }

        public readonly string[] ForbiddenWords =
        {
            "violencia",
            "odio",
            "grosería",
            "discriminación",
            "basura",
            "idiota",
            "estupido"
        };

        public bool ContainsForbiddenContent(string? text)
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

