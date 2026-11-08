using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using OmniReserve.Application.Rooms.Commands.CreateRoom;
using OmniReserve.Application.Rooms.Queries.GetRoomById;
using OmniReserve.Application.Rooms.Queries.GetAllRooms;

namespace OmniReserve.Api.Controllers;

[Authorize]
[EnableRateLimiting("GlobalLimit")]
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

    // Obtener todas las habitaciones con Redis
    [HttpGet]
    public async Task<IActionResult> GetAllRooms()
    {
        var result = await _sender.Send(new GetAllRoomsQuery());

        return Ok(result);
    }
}