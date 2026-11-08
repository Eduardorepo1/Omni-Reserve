using MediatR;
using OmniReserve.Application.Rooms.Queries.GetRoomById;

namespace OmniReserve.Application.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQuery : IRequest<List<RoomResponseDto>>
{
}