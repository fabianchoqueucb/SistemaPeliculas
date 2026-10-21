using Microsoft.AspNetCore.Mvc;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        // 1. GET: Obtener todas las películas (simulado)
        [HttpGet]
        public IActionResult GetMovies()
        {
            var movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Title = "Inception",
                    Description = "Una película sobre sueños dentro de sueños.",
                    ReleaseDate = new DateTime(2010, 7, 16),
                    Length = "2h 28m",
                    Genre = "Ciencia Ficción"
                },
                new Movie
                {
                    Id = 2,
                    Title = "Interstellar",
                    Description = "Un viaje a través de un agujero de gusano en el espacio.",
                    ReleaseDate = new DateTime(2014, 11, 7),
                    Length = "2h 49m",
                    Genre = "Aventura"
                }
            };

            return Ok(movies);
        }

        // 2. GET por ID: Obtener una película específica
        [HttpGet("{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movie = new Movie
            {
                Id = id,
                Title = "Película de Ejemplo",
                Description = "Descripción de prueba buscada por ID",
                ReleaseDate = DateTime.Now,
                Length = "2h 00m",
                Genre = "Drama"
            };

            return Ok(movie);
        }

        // 3. POST: Crear una nueva película recibiendo un JSON desde Postman
        [HttpPost]
        public IActionResult CreateMovie([FromBody] Movie nuevaPelicula)
        {
            nuevaPelicula.Id = 10;

            return Ok(new
            {
                mensaje = "Película creada exitosamente",
                data = nuevaPelicula
            });
        }
    }
}