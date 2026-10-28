using System.Security.Cryptography;
using SistemaPeliculas.Core.Interfaces;

namespace SistemaPeliculas.Infrastructure.Services
{
    // Guarda las contraseñas como hash PBKDF2 con el formato ITERACIONES.SAL.HASH
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;      // bytes de sal aleatoria
        private const int KeySize = 32;       // bytes del hash resultante
        private const int Iterations = 10000; // veces que se repite el cálculo

        public string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, KeySize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.');
            if (parts.Length != 3)
                return false;

            var iterations = int.Parse(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            byte[] keyToCheck = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, key.Length);

            // Compara en tiempo constante para no dar pistas por el tiempo de respuesta
            return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
        }
    }
}
