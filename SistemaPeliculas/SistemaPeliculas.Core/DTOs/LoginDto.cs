namespace SistemaPeliculas.Core.DTOs
{
    // Lo que envía el cliente para iniciar sesión
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
