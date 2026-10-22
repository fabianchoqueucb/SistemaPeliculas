namespace SistemaPeliculas.Core.Entities
{
    public class Review : BaseEntity
    {
        public int? MovieId { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public float Grade { get; set; }
        public virtual Movie? Movie { get; set; }
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
