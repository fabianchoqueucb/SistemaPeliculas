using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Services.Interfaces
{
    public interface IActorService
    {
        Task<IEnumerable<Actor>> GetActorsAsync(string? name = null, int? movieId = null);
        Task<Actor?> GetActorByIdAsync(int id);
        Task InsertActor(Actor actor);
        Task UpdateActor(Actor actor);
        Task DeleteActor(int id);
    }
}

