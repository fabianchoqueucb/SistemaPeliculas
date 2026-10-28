using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Exceptions;
using SistemaPeliculas.Core.Interfaces;

namespace SistemaPeliculas.Core.Services
{
    // CU-01 Iniciar sesión: verifica correo y contraseña
    public class SecurityService : ISecurityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public SecurityService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<User> ValidateCredentialsAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new BusinessException("El correo y la contraseña son obligatorios");

            var user = await _userRepository.GetUserByEmailAsync(email.Trim());

            // Mismo mensaje si falla el correo o la contraseña: no revelamos cuál de los dos es
            if (user == null || !_passwordService.Check(user.Password, password))
                throw new BusinessException("Correo o contraseña incorrectos", 401);

            if (!user.IsActive)
                throw new BusinessException("La cuenta está inactiva", 403);

            return user;
        }
    }
}
