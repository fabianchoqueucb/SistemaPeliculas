using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPeliculas.Core.DTOs;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;

namespace SistemaPeliculas.Api.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public ReviewController(IReviewService reviewService, IMapper mapper)
        {
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // GET api/reviews
        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
        }

        // GET api/reviews/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            return Ok(_mapper.Map<ReviewDto>(review));
        }

        // POST api/reviews  (CU-03 Subir reseña) - cualquier usuario con sesión iniciada
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InsertReview(ReviewDto reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            // El autor es quien inició sesión, no el que venga en el body: nadie puede reseñar a nombre de otro
            review.UserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var created = await _reviewService.InsertReviewAsync(review);
            var result = _mapper.Map<ReviewDto>(created);
            return CreatedAtAction(nameof(GetReviewById), new { id = result.Id }, result);
        }
    }
}
