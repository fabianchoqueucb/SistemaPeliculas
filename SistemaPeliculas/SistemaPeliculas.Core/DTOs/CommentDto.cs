namespace SistemaPeliculas.Core.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int? ReviewId { get; set; }
        public int? UserId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}

