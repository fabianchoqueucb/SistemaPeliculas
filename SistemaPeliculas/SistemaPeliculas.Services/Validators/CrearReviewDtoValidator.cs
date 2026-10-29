using FluentValidation;
using SistemaPeliculas.Core.DTOs;

namespace SistemaPeliculas.Services.Validators
{
    public class CrearReviewDtoValidator : AbstractValidator<ReviewDto>
    {
        public CrearReviewDtoValidator()
        {
            RuleFor(x => x.Id)
                .Equal(0).When(x => x.Id != 0)
                .WithMessage("El ID debe ser 0 o no enviarse para la creación de una nueva reseña.");

            RuleFor(x => x.MovieId)
                .GreaterThan(0).WithMessage("El ID de la película es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID del usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Grade)
                .InclusiveBetween(1f, 10f).WithMessage("La calificación debe estar entre 1 y 10.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción no puede exceder los 1000 caracteres.");
        }
    }
}

