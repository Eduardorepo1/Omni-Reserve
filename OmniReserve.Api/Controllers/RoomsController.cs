using MediatR;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Application.Rooms.Commands.CreateRoom;

namespace OmniReserve.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ISender _sender;

    public RoomsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRoom(CreateRoomCommand command)
    {
        var resultado = await _sender.Send(command);

        return Ok(resultado);
    }
}