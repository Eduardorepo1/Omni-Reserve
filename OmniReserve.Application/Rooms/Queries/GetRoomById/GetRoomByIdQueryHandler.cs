using MediatR;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler
    : IRequestHandler<GetRoomByIdQuery, RoomResponseDto>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomByIdQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<RoomResponseDto> Handle(
        GetRoomByIdQuery request,
        CancellationToken cancellationToken)
    {
        var room = await _roomRepository.GetByIdAsync(request.RoomId);

        if (room is null)
        {
            throw new Exception("Room not found");
        }

        return new RoomResponseDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            Type = room.Type.ToString(),
            Price = room.PricePerNight,
            IsAvailable = room.IsAvailable
        };
    }
}