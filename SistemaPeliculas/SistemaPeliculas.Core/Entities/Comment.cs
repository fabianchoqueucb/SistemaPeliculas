namespace SistemaPeliculas.Core.Entities
{
    public class Comment : BaseEntity
    {
        public int? ReviewId { get; set; }
        public string Description { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool? IsActive { get; set; }
        public virtual Review? Review { get; set; }
    }
}
