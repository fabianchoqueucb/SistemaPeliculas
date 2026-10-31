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
        public virtual ICollection<Actor> Actors { get; set; } = new List<Actor>();
        public virtual ICollection<Watchlist> Watchlists { get; set; } = new List<Watchlist>();
    }
}
