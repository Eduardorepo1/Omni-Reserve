namespace OmniReserve.Application.Rooms.Queries.GetRoomById;

public class RoomResponseDto
{
    public Guid Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
}