namespace SistemaPeliculas.Core.Entities
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string Length { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
