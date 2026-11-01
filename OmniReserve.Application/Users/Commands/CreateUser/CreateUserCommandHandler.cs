using MediatR;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler
    : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
            throw new ArgumentException("El nombre es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.LastName))
            throw new ArgumentException("El apellido es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("El correo electrónico es obligatorio.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("La contraseña es obligatoria.");

        // Encriptar la contraseña antes de guardar al usuario.
        string passwordHash = _passwordHasher.Hash(request.Password);

        // Crear el usuario con el rol Client por defecto.
        var user = new User(
            request.FirstName,
            request.LastName,
            new EmailAddress(request.Email),
            passwordHash);

        // Guardar el usuario en la base de datos.
        await _userRepository.AddAsync(user);

        return user.Id;
    }
}