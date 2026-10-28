using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface ISecurityService
    {
        Task<User> ValidateCredentialsAsync(string email, string password);
    }
}
