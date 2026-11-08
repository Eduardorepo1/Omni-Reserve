namespace OmniReserve.Domain.Exceptions;

public class InvalidCredentialsException : DomainException
{
    public InvalidCredentialsException()
        : base("Credenciales inválidas")
    {
    }
}