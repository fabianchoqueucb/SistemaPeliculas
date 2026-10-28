using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SistemaPeliculas.Core.DTOs;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;

namespace SistemaPeliculas.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISecurityService _securityService;
        private readonly IConfiguration _configuration;

        public AuthController(ISecurityService securityService, IConfiguration configuration)
        {
            _securityService = securityService;
            _configuration = configuration;
        }

        // POST api/auth/login  (CU-01 Iniciar sesión)
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _securityService.ValidateCredentialsAsync(loginDto.Email, loginDto.Password);
            return Ok(GenerateToken(user));
        }

        private TokenDto GenerateToken(User user)
        {
            // 1. Clave secreta con la que se firma el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 2. Claims: los datos del usuario que viajan dentro del token
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // 3. El token: quién lo emite, para quién es, los claims y cuándo vence
            var expiration = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Authentication:ExpirationMinutes"]!));
            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                UserId = user.Id,
                Name = $"{user.FirstName} {user.LastName}",
                Role = user.Role
            };
        }
    }
}
