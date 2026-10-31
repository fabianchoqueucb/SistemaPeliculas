using FluentValidation;
using SistemaPeliculas.Core.DTOs;

namespace SistemaPeliculas.Services.Validators
{
    public class CrearWatchlistDtoValidator : AbstractValidator<CrearWatchlistDto>
    {
        public CrearWatchlistDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID de usuario es obligatorio");

            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("El ID de la película es obligatorio");
        }
    }
}

