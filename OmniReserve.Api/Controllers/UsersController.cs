using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Application.Users.Commands.CreateUser;
using OmniReserve.Application.Users.Commands.LoginUser;

namespace OmniReserve.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserCommand command)
    {
        var result = await _sender.Send(command);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginUserCommand command)
    {
        var token = await _sender.Send(command);

        return Ok(new { Token = token });
    }
}