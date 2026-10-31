namespace SistemaPeliculas.Core.DTOs
{
    public class ActorDto
    {
        public int Id { get; set; }
        public int? MovieId { get; set; }
        public string? MovieTitle { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

