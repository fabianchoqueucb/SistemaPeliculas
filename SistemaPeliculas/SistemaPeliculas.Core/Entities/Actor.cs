namespace SistemaPeliculas.Core.Entities
{
    public partial class Actor : BaseEntity
    {
        public int? MovieId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual Movie? Movie { get; set; }
    }
}

