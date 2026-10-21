using Microsoft.AspNetCore.Mvc;
using SistemaPeliculas.Core.Entities;

namespace SistemaPeliculas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        // 1. GET: Obtener todos los comentarios (simulado)
        [HttpGet]
        public IActionResult GetComments()
        {
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    ReviewId = 1,
                    Description = "¡Totalmente de acuerdo con la reseña!",
                    Date = DateTime.Now,
                    IsActive = true
                },
                new Comment
                {
                    Id = 2,
                    ReviewId = 1,
                    Description = "A mí no me gustó tanto el final.",
                    Date = DateTime.Now,
                    IsActive = true
                }
            };

            return Ok(comments);
        }

        // 2. GET por ID: Obtener un comentario específico
        [HttpGet("{id}")]
        public IActionResult GetCommentById(int id)
        {
            var comment = new Comment
            {
                Id = id,
                ReviewId = 1,
                Description = "Comentario de prueba buscado por ID",
                Date = DateTime.Now,
                IsActive = true
            };

            return Ok(comment);
        }

        // 3. POST: Crear un nuevo comentario recibiendo un JSON desde Postman
        [HttpPost]
        public IActionResult CreateComment([FromBody] Comment nuevoComentario)
        {
            nuevoComentario.Id = 50;
            nuevoComentario.Date = DateTime.Now;

            return Ok(new
            {
                mensaje = "Comentario creado exitosamente",
                data = nuevoComentario
            });
        }
    }
}
