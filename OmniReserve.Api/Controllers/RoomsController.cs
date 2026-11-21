using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using OmniReserve.Application.Rooms.Commands.CreateRoom;
using OmniReserve.Application.Rooms.Queries.GetRoomById;
using OmniReserve.Application.Rooms.Queries.GetAllRooms;

namespace OmniReserve.Api.Controllers;

/// <summary>
/// Controlador encargado de administrar las habitaciones del hotel.
/// </summary>
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

    /// <summary>
    /// Crea una nueva habitación en el catálogo del hotel.
    /// </summary>
    /// <param name="command">Información necesaria para registrar la habitación.</param>
    /// <returns>El identificador único de la habitación creada.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CreateRoom(CreateRoomCommand command)
    {
        var resultado = await _sender.Send(command);

        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene los detalles de una habitación mediante su identificador.
    /// </summary>
    /// <param name="id">Identificador único de la habitación.</param>
    /// <returns>Los datos de la habitación solicitada.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RoomResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> GetRoom(Guid id)
    {
        var query = new GetRoomByIdQuery { RoomId = id };

        var result = await _sender.Send(query);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Obtiene el catálogo completo de habitaciones del hotel.
    /// </summary>
    /// <returns>Una lista con las habitaciones registradas y su información.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<RoomResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> GetAllRooms()
    {
        var result = await _sender.Send(new GetAllRoomsQuery());

        return Ok(result);
    }
}