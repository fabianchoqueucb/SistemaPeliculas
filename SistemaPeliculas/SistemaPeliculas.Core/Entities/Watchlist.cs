namespace SistemaPeliculas.Core.Entities
{
    public partial class Watchlist : BaseEntity
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime DateAdded { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
    }
}

