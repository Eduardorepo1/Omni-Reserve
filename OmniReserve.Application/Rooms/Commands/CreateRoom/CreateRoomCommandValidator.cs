using FluentValidation;

namespace OmniReserve.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty().WithMessage("El número de la habitación es obligatorio.")
            .MaximumLength(5).WithMessage("El número de la habitación debe tener máximo 5 caracteres.");

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0).WithMessage("El precio por la noche debe ser mayor a 0.");
    }
}