namespace SistemaPeliculas.Core.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public string? Description { get; set; }
        public float Grade { get; set; }
        public DateTime Date { get; set; }
    }
}
