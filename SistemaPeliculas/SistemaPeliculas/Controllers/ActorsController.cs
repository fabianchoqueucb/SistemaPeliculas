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
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;
        private readonly IMapper _mapper;
        private readonly CrearActorDtoValidator _crearValidator;
        private readonly ActualizarActorDtoValidator _actualizarValidator;

        public ActorsController(
            IMapper mapper,
            IActorService actorService,
            CrearActorDtoValidator crearValidator,
            ActualizarActorDtoValidator actualizarValidator)
        {
            _actorService = actorService;
            _mapper = mapper;
            _crearValidator = crearValidator;
            _actualizarValidator = actualizarValidator;
        }

        [HttpGet]
        public async Task<IActionResult> GetActors([FromQuery] string? name, [FromQuery] int? movieId)
        {
            var actors = await _actorService.GetActorsAsync(name, movieId);
            var actorDtos = _mapper.Map<IEnumerable<ActorDto>>(actors);
            var response = new ApiResponse<IEnumerable<ActorDto>>(actorDtos);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActorById(int id)
        {
            var actor = await _actorService.GetActorByIdAsync(id);
            if (actor == null)
            {
                return NotFound(new { message = "Actor no encontrado." });
            }

            var actorDto = _mapper.Map<ActorDto>(actor);
            return Ok(new ApiResponse<ActorDto>(actorDto));
        }

        [HttpPost]
        public async Task<IActionResult> InsertActor([FromBody] ActorDto actorDto)
        {
            var validationResult = await _crearValidator.ValidateAsync(actorDto);
            if (!validationResult.IsValid)
            {
                throw new BusinessException(message: "Error de validación", details: validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }

            var actor = _mapper.Map<Actor>(actorDto);
            await _actorService.InsertActor(actor);

            var resultDto = _mapper.Map<ActorDto>(actor);
            return CreatedAtAction(nameof(GetActorById), new { id = actor.Id }, new ApiResponse<ActorDto>(resultDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateActor(int id, [FromBody] ActorDto actorDto)
        {
            if (id != actorDto.Id)
            {
                throw new BusinessException("El ID del actor no coincide.");
            }

            var validationResult = await _actualizarValidator.ValidateAsync(actorDto);
            if (!validationResult.IsValid)
            {
                throw new BusinessException(message: "Error de validación", details: validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }

            var actor = _mapper.Map<Actor>(actorDto);
            await _actorService.UpdateActor(actor);

            var resultDto = _mapper.Map<ActorDto>(actor);
            return Ok(new ApiResponse<ActorDto>(resultDto));
        }
    }
}

