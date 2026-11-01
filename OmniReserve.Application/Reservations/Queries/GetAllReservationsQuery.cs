using MediatR;
using OmniReserve.Application.Reservations.Queries.GetReservationDetails;

namespace OmniReserve.Application.Reservations.Queries.GetAllReservations;

public class GetAllReservationsQuery : IRequest<List<ReservationDetailsDto>>
{
}