namespace SistemaPeliculas.Core.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!; // hash PBKDF2, nunca el texto plano
        public DateTime DateOfBirth { get; set; }
        public string? Telephone { get; set; }
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
