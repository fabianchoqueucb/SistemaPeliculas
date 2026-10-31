using System.Net;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Exceptions;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Services.Interfaces;

namespace SistemaPeliculas.Services.Services
{
    public class WatchlistService : IWatchlistService
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Movie> _movieRepository;

        public WatchlistService(
            IWatchlistRepository watchlistRepository,
            IBaseRepository<User> userRepository,
            IBaseRepository<Movie> movieRepository)
        {
            _watchlistRepository = watchlistRepository;
            _userRepository = userRepository;
            _movieRepository = movieRepository;
        }

        public async Task<Watchlist> AddToWatchlistAsync(int userId, int movieId)
        {
            // 1. Validar que el usuario exista
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new BusinessException("El usuario no existe");
            }

            // 2. Validar que la película exista
            var movie = await _movieRepository.GetById(movieId);
            if (movie == null)
            {
                throw new BusinessException("La película no existe");
            }

            // 3. Validar que no esté duplicado en la lista del usuario
            if (await _watchlistRepository.ExistsAsync(userId, movieId))
            {
                throw new BusinessException("La película ya se encuentra en la lista de seguimiento del usuario");
            }

            var watchlist = new Watchlist
            {
                UserId = userId,
                MovieId = movieId,
                DateAdded = DateTime.Now
            };

            await _watchlistRepository.Add(watchlist);

            return await _watchlistRepository.GetByIdWithDetailsAsync(watchlist.Id) ?? watchlist;
        }

        public async Task<IEnumerable<Watchlist>> GetWatchlistByUserIdAsync(int userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new BusinessException("El usuario no existe");
            }

            return await _watchlistRepository.GetByUserIdAsync(userId);
        }

        public async Task DeleteWatchlistAsync(int id)
        {
            var watchlist = await _watchlistRepository.GetById(id);
            if (watchlist == null)
            {
                throw new BusinessException("El registro en la lista de seguimiento no existe", HttpStatusCode.NotFound);
            }

            await _watchlistRepository.Delete(id);
        }
    }
}

