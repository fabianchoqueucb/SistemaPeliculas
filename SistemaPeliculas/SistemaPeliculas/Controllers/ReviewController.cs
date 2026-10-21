using Microsoft.AspNetCore.Mvc;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        // 1. GET: Obtener todas las reviews (simulado)
        [HttpGet]
        public IActionResult GetReviews()
        {
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = 1,
                    MovieId = 1,
                    Description = "Excelente película, muy recomendada.",
                    Date = DateTime.Now,
                    Grade = 4.5f
                },
                new Review
                {
                    Id = 2,
                    MovieId = 1,
                    Description = "Entretenida pero pudo ser mejor.",
                    Date = DateTime.Now,
                    Grade = 3.0f
                }
            };

            return Ok(reviews);
        }

        // 2. GET por ID: Obtener una review específica
        [HttpGet("{id}")]
        public IActionResult GetReviewById(int id)
        {
            var review = new Review
            {
                Id = id,
                MovieId = 1,
                Description = "Reseña de prueba buscada por ID",
                Date = DateTime.Now,
                Grade = 5.0f
            };

            return Ok(review);
        }

        // 3. POST: Crear una nueva review recibiendo un JSON desde Postman
        [HttpPost]
        public IActionResult CreateReview([FromBody] Review nuevaReview)
        {
            
            nuevaReview.Id = 99; 
            nuevaReview.Date = DateTime.Now;

            return Ok(new
            {
                mensaje = "Review creada exitosamente",
                data = nuevaReview
            });
        }
    }
}