using FluentValidation;
using SistemaPeliculas.Core.DTOs;

namespace SistemaPeliculas.Services.Validators
{
    public class ActualizarActorDtoValidator : AbstractValidator<ActorDto>
    {
        public ActualizarActorDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("El ID del actor debe ser mayor a 0");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MaximumLength(100).WithMessage("El nombre no puede superar 100 caracteres");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio")
                .MaximumLength(100).WithMessage("El apellido no puede superar 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio")
                .EmailAddress().WithMessage("El correo electrónico debe tener un formato válido")
                .MaximumLength(100).WithMessage("El correo electrónico no puede superar 100 caracteres");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("La fecha de nacimiento es obligatoria")
                .Must(d => d <= DateTime.Today && d >= new DateTime(1850, 1, 1))
                .WithMessage("La fecha de nacimiento no es válida");
        }
    }
}

