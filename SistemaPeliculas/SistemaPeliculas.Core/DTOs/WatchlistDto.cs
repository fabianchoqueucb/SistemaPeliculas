namespace SistemaPeliculas.Core.DTOs
{
    public class WatchlistDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int MovieId { get; set; }
        public string? MovieTitle { get; set; }
        public string? MovieGenre { get; set; }
        public DateTime? MovieReleaseDate { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

