using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaPeliculas.Core.DTOs;
using SistemaPeliculas.Core.Entities;
using SistemaPeliculas.Core.Interfaces;
using SistemaPeliculas.Core.QueryFilters;

namespace SistemaPeliculas.Api.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public MovieController(IMovieService movieService, IReviewService reviewService, IMapper mapper)
        {
            _movieService = movieService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // GET api/movies?title=&genre=&year=  (CU-04 Consultar catálogo) - público
        [HttpGet]
        public async Task<IActionResult> GetMovies([FromQuery] MovieQueryFilter filter)
        {
            var movies = await _movieService.GetMoviesAsync(filter);
            return Ok(_mapper.Map<IEnumerable<MovieDto>>(movies));
        }

        // GET api/movies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            return Ok(_mapper.Map<MovieDto>(movie));
        }

        // GET api/movies/5/reviews  (CU-05 Consultar reseñas de una película) - público
        [HttpGet("{id}/reviews")]
        public async Task<IActionResult> GetMovieReviews(int id)
        {
            var summary = await _reviewService.GetReviewsByMovieAsync(id);
            return Ok(_mapper.Map<MovieReviewsDto>(summary));
        }

        // POST api/movies  (CU-02 Subir película) - solo el moderador (RN-12)
        [HttpPost]
        [Authorize(Roles = "Moderador")]
        public async Task<IActionResult> InsertMovie(MovieDto movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            var created = await _movieService.InsertMovieAsync(movie);
            var result = _mapper.Map<MovieDto>(created);
            return CreatedAtAction(nameof(GetMovieById), new { id = result.Id }, result);
        }
    }
}
