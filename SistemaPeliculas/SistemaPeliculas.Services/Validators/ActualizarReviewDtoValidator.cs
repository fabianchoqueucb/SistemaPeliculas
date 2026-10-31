using FluentValidation;
using SistemaPeliculas.Core.DTOs;

namespace SistemaPeliculas.Services.Validators
{
    public class ActualizarReviewDtoValidator : AbstractValidator<ReviewDto>
    {
        public ActualizarReviewDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID de la reseña es obligatorio y debe ser mayor que cero.");

            // Se usa para comprobar que quien edita es el autor de la reseña
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("El ID del usuario es obligatorio y debe ser mayor que cero.");

            RuleFor(x => x.Grade)
                .InclusiveBetween(1f, 10f).WithMessage("La calificación debe estar entre 1 y 10.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La descripción no puede exceder los 1000 caracteres.");
        }
    }
}
