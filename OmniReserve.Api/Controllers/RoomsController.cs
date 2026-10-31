using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OmniReserve.Application.Rooms.Commands.CreateRoom;
using OmniReserve.Application.Rooms.Queries.GetRoomById;

namespace OmniReserve.Api.Controllers;

[Authorize]
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoom(Guid id)
    {
        var query = new GetRoomByIdQuery { RoomId = id };

        var result = await _sender.Send(query);

        return Ok(result);
    }
}