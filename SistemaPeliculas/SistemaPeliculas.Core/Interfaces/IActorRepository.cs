using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Core.Interfaces
{
    public interface IActorRepository : IBaseRepository<Actor>
    {
        Task<IEnumerable<Actor>> GetActorsAsync(string? name = null, int? movieId = null);
        Task<Actor?> GetActorWithDetailsAsync(int id);
    }
}

