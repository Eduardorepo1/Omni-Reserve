using MediatR;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Reservations.Queries.GetReservationDetails;

public class GetReservationDetailsQueryHandler
    : IRequestHandler<GetReservationDetailsQuery, ReservationDetailsDto>
{
    private readonly IReservationRepository _repository;

    public GetReservationDetailsQueryHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReservationDetailsDto> Handle(
        GetReservationDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var reservation = await _repository
            .GetReservationWithDetailsAsync(request.ReservationId);

        if (reservation is null)
        {
            throw new Exception("Reservation not found");
        }

        return new ReservationDetailsDto
        {
            Id = reservation.Id,
            UserEmail = reservation.User.Email,
            RoomNumber = reservation.Room.RoomNumber,
            StartDate = reservation.CheckInDate,
            EndDate = reservation.CheckOutDate
        };
    }
}