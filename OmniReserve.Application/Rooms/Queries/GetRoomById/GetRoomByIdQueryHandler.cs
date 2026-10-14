using MediatR;

namespace OmniReserve.Application.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler
    : IRequestHandler<GetRoomByIdQuery, RoomResponseDto>
{
    public Task<RoomResponseDto> Handle(
        GetRoomByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Respuesta de prueba: todavia no hay base de datos
        var response = new RoomResponseDto
        {
            Id = request.RoomId,
            RoomNumber = "101",
            Type = "Single",
            Price = 100m,
            IsAvailable = true
        };

        return Task.FromResult(response);
    }
}