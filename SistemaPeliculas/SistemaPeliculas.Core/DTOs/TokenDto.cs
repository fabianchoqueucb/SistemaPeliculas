namespace SistemaPeliculas.Core.DTOs
{
    // Lo que devuelve la API cuando el login es correcto
    public class TokenDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
