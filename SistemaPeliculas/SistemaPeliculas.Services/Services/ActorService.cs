using System.Net;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Exceptions;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Services.Interfaces;

namespace SistemaPeliculas.Services.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorRepository _actorRepository;
        private readonly IBaseRepository<Movie> _movieRepository;

        public ActorService(IActorRepository actorRepository, IBaseRepository<Movie> movieRepository)
        {
            _actorRepository = actorRepository;
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<Actor>> GetActorsAsync(string? name = null, int? movieId = null)
        {
            return await _actorRepository.GetActorsAsync(name, movieId);
        }

        public async Task<Actor?> GetActorByIdAsync(int id)
        {
            return await _actorRepository.GetActorWithDetailsAsync(id);
        }

        public async Task InsertActor(Actor actor)
        {
            // Regla de negocio: Si se asocia a una película, el MovieId debe existir
            if (actor.MovieId.HasValue && actor.MovieId.Value > 0)
            {
                var movie = await _movieRepository.GetById(actor.MovieId.Value);
                if (movie == null)
                {
                    throw new BusinessException("La película especificada no existe");
                }
            }

            await _actorRepository.Add(actor);
        }

        public async Task UpdateActor(Actor actor)
        {
            var existingActor = await _actorRepository.GetById(actor.Id);
            if (existingActor == null)
            {
                throw new BusinessException("El actor no existe");
            }

            // Regla de negocio: Si se asocia a una película, el MovieId debe existir
            if (actor.MovieId.HasValue && actor.MovieId.Value > 0)
            {
                var movie = await _movieRepository.GetById(actor.MovieId.Value);
                if (movie == null)
                {
                    throw new BusinessException("La película especificada no existe");
                }
            }

            // Actualizar campos sin modificar el ID
            existingActor.FirstName = actor.FirstName;
            existingActor.LastName = actor.LastName;
            existingActor.Email = actor.Email;
            existingActor.DateOfBirth = actor.DateOfBirth;
            existingActor.MovieId = actor.MovieId;
            existingActor.IsActive = actor.IsActive;

            await _actorRepository.Update(existingActor);
        }

        public async Task DeleteActor(int id)
        {
            var actor = await _actorRepository.GetById(id);
            if (actor == null)
            {
                throw new BusinessException("El actor no existe", HttpStatusCode.NotFound);
            }

            await _actorRepository.Delete(id);
        }
    }
}

