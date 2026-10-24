using MediatR;
using OmniReserve.Application.Common.Interfaces;

namespace OmniReserve.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler
    : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<string> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            throw new Exception("Credenciales inválidas");
        }

        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            throw new Exception("Credenciales inválidas");

        bool passwordIsValid = _passwordHasher.Verify(
            request.Password,
            user.PasswordHash);

        if (!passwordIsValid)
            throw new Exception("Credenciales inválidas");

        return _jwtProvider.Generate(user);
    }
}