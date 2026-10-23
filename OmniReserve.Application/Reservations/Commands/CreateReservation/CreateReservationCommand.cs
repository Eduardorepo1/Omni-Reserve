using MediatR;

namespace OmniReserve.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }

    public Guid RoomId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }
}