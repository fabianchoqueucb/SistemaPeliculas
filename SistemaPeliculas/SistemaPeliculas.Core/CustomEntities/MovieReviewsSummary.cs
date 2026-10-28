using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.CustomEntities
{
    // Resultado del CU-05: la película, sus reseñas y el promedio calculado por el servicio
    public class MovieReviewsSummary
    {
        public Movie Movie { get; set; } = null!;
        public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
        public double? AverageGrade { get; set; }   // null si todavía no tiene reseñas
        public int TotalReviews { get; set; }
    }
}
