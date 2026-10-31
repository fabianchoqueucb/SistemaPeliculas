using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SistemaPeliculas.Api.Response;
using SistemaPeliculas.Core.DTOs;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Exceptions;
using SistemaPeliculas.Services.Interfaces;
using SistemaPeliculas.Services.Validators;

namespace SistemaPeliculas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly CrearMovieDtoValidator _crearValidator;
        private readonly ActualizarMovieDtoValidator _actualizarValidator;

        public MovieController(IMapper mapper, IMovieService movieService,
            CrearMovieDtoValidator crearValidator,
            ActualizarMovieDtoValidator actualizarValidator)
        {
            _movieService = movieService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery] string? title, [FromQuery] string? genre)
        {
            var movies = await _movieService.GetAllMoviesAsync(title, genre);
            var movieDtos = _mapper.Map<IEnumerable<MovieDto>>(movies);
            var response = new ApiResponse<IEnumerable<MovieDto>>(movieDtos);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound(new { message = "Película no encontrada." });

            var movieDto = _mapper.Map<MovieDto>(movie);
            var response = new ApiResponse<MovieDto>(movieDto);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertMovie(MovieDto movieDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(movieDto);
            if (!validationResult.IsValid)
            {
                throw new BusinessException(message: "Error de validación", details: validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }

            try
            {
                var movie = _mapper.Map<Movie>(movieDto);
                await _movieService.InsertMovie(movie);

                var resultDto = _mapper.Map<MovieDto>(movie);
                var response = new ApiResponse<MovieDto>(resultDto);
                return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la película", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie(int id, [FromBody] MovieDto movieDto)
        {
            if (id != movieDto.Id)
                throw new BusinessException("El ID de la película no coincide.");

            var validationResult = await _actualizarValidator.ValidateAsync(movieDto);
            if (!validationResult.IsValid)
            {
                throw new BusinessException(message: "Error de validación", details: validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }

            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound(new { message = "Película no encontrada." });

            try
            {
                _mapper.Map(movieDto, movie);
                await _movieService.UpdateMovie(movie);

                var resultDto = _mapper.Map<MovieDto>(movie);
                return Ok(new ApiResponse<MovieDto>(resultDto));
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar la película", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (id <= 0)
                throw new BusinessException("El ID debe ser mayor que cero.");

            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound(new { message = "Película no encontrada." });

            try
            {
                await _movieService.DeleteMovie(id);
                return NoContent();
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la película", error = ex.Message });
            }
        }
    }
}
