using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaPeliculas.Api.Response;
using SistemaPeliculas.Core.DTOs;
using SistemaPeliculas.Core.Exceptions;
using SistemaPeliculas.Services.Interfaces;
using SistemaPeliculas.Services.Validators;

namespace SistemaPeliculas.Api.Controllers
{
    [Route("api/watchlists")]
    [ApiController]
    public class WatchlistsController : ControllerBase
    {
        private readonly IWatchlistService _watchlistService;
        private readonly IMapper _mapper;
        private readonly CrearWatchlistDtoValidator _crearValidator;

        public WatchlistsController(
            IMapper mapper,
            IWatchlistService watchlistService,
            CrearWatchlistDtoValidator crearValidator)
        {
            _watchlistService = watchlistService;
            _mapper = mapper;
            _crearValidator = crearValidator;
        }

        [HttpPost]
        public async Task<IActionResult> AddToWatchlist([FromBody] CrearWatchlistDto dto)
        {
            var validationResult = await _crearValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new BusinessException(message: "Error de validación", details: validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }

            var watchlist = await _watchlistService.AddToWatchlistAsync(dto.UserId, dto.MovieId);
            var resultDto = _mapper.Map<WatchlistDto>(watchlist);

            return Created($"api/watchlists/{resultDto.Id}", new ApiResponse<WatchlistDto>(resultDto));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetWatchlistByUser(int userId)
        {
            if (userId <= 0)
            {
                throw new BusinessException("El ID de usuario debe ser mayor a 0");
            }

            var watchlist = await _watchlistService.GetWatchlistByUserIdAsync(userId);
            var dtos = _mapper.Map<IEnumerable<WatchlistDto>>(watchlist);

            return Ok(new ApiResponse<IEnumerable<WatchlistDto>>(dtos));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFromWatchlist(int id)
        {
            if (id <= 0)
            {
                throw new BusinessException("El ID debe ser mayor a 0");
            }

            await _watchlistService.DeleteWatchlistAsync(id);
            return NoContent();
        }
    }
}

