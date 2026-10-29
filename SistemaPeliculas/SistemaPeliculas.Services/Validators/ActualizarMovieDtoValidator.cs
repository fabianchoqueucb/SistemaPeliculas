using FluentValidation;
using SistemaPeliculas.Core.DTOs;

namespace SistemaPeliculas.Services.Validators
{
    public class ActualizarMovieDtoValidator : AbstractValidator<MovieDto>
    {
        public ActualizarMovieDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("El título es obligatorio.")
                .Length(2, 100).WithMessage("El título debe tener entre 2 y 100 caracteres.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.")
                .MaximumLength(1000).WithMessage("La descripción no puede exceder los 1000 caracteres.");

            RuleFor(x => x.Genre)
                .NotEmpty().WithMessage("El género es obligatorio.");

            RuleFor(x => x.Length)
                .NotEmpty().WithMessage("La duración es obligatoria.");
        }
    }
}

