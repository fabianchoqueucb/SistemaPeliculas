using FluentValidation;
using SistemaPeliculas.Core.DTOs;

namespace SistemaPeliculas.Services.Validators
{
    public class CrearMovieDtoValidator : AbstractValidator<MovieDto>
    {
        public CrearMovieDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).When(x => x.Id != 0)
                .WithMessage("El ID debe ser 0 o no enviarse para la creación de una nueva película.");

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

