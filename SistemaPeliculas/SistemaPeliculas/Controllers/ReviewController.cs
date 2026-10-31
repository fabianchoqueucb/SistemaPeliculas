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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;
        private readonly CrearReviewDtoValidator _crearValidator;

        public ReviewController(IMapper mapper, IReviewService reviewService,
            CrearReviewDtoValidator crearValidator)
        {
            _reviewService = reviewService;
            _mapper = mapper;
            _crearValidator = crearValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            var response = new ApiResponse<IEnumerable<ReviewDto>>(reviewDtos);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound(new { message = "Reseña no encontrada." });

            var reviewDto = _mapper.Map<ReviewDto>(review);
            var response = new ApiResponse<ReviewDto>(reviewDto);
            return Ok(response);
        }

        [HttpGet("movie/{movieId}")]
        public async Task<IActionResult> GetReviewsByMovie(int movieId)
        {
            var reviews = await _reviewService.GetReviewsByMovieAsync(movieId);
            var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            var response = new ApiResponse<IEnumerable<ReviewDto>>(reviewDtos);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> InsertReview(ReviewDto reviewDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(reviewDto);
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
                var review = _mapper.Map<Review>(reviewDto);
                await _reviewService.InsertReview(review);

                var resultDto = _mapper.Map<ReviewDto>(review);
                var response = new ApiResponse<ReviewDto>(resultDto);
                return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, response);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al crear la reseña", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            if (id <= 0)
                throw new BusinessException("El ID debe ser mayor que cero.");

            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound(new { message = "Reseña no encontrada." });

            try
            {
                await _reviewService.DeleteReview(id);
                return NoContent();
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar la reseña", error = ex.Message });
            }
        }
    }
}
