using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Application.Reservations.Queries.GetReservationDetails;

namespace OmniReserve.Application.Reservations.Queries.GetAllReservations;

public class GetAllReservationsQueryHandler
    : IRequestHandler<GetAllReservationsQuery, List<ReservationDetailsDto>>
{
    private readonly IReservationRepository _reservationRepository;

    public GetAllReservationsQueryHandler(
        IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public async Task<List<ReservationDetailsDto>> Handle(
        GetAllReservationsQuery request,
        CancellationToken cancellationToken)
    {
        var reservations = await _reservationRepository.GetAllAsync();

        return reservations.Select(r => new ReservationDetailsDto
        {
            Id = r.Id,
            UserEmail = r.User.Email.Value,
            RoomNumber = r.Room.RoomNumber,
            StartDate = r.CheckInDate,
            EndDate = r.CheckOutDate
        }).ToList();
    }
}