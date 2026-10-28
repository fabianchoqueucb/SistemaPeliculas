namespace SistemaPeliculas.Core.DTOs
{
    // Respuesta de GET api/movies/{id}/reviews
    public class MovieReviewsDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = null!;
        public double? AverageGrade { get; set; }
        public int TotalReviews { get; set; }
        public IEnumerable<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}
