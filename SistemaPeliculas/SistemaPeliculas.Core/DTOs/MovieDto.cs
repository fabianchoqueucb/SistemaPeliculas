namespace SistemaPeliculas.Core.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string Length { get; set; } = null!;
        public string Genre { get; set; } = null!;
    }
}
