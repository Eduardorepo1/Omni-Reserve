namespace OmniReserve.Domain.Exceptions;

public class RoomNotAvailableException : DomainException
{
    public RoomNotAvailableException(string roomNumber)
        : base($"La habitación {roomNumber} no está disponible para reserva.")
    {
    }
}