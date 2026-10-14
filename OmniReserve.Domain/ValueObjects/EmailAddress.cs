namespace OmniReserve.Domain.ValueObjects;

public record EmailAddress
{
    public string Value { get; }

    public EmailAddress(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
        {
            throw new ArgumentException("El formato del correo es inválido");
        }

        Value = email;
    }
}