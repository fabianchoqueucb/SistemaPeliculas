using Microsoft.EntityFrameworkCore;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Infrastructure.Data;

namespace SistemaPeliculas.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PeliculasContext _context;

        public UserRepository(PeliculasContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
