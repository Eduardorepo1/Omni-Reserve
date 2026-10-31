namespace OmniReserve.Api.Requests;

public class CreateReservationRequest
{
    public Guid RoomId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public decimal TotalPrice { get; set; }
}