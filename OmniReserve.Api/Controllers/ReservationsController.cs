using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Api.Requests;
using OmniReserve.Application.Reservations.Commands.CreateReservation;
using OmniReserve.Application.Reservations.Queries.GetAllReservations;
using OmniReserve.Domain.Constants;

namespace OmniReserve.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ISender _sender;

    public ReservationsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(
        [FromBody] CreateReservationRequest request)
    {
        var userIdClaim = HttpContext.User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var command = new CreateReservationCommand
        {
            UserId = userId,
            RoomId = request.RoomId,
            StartDate = request.StartDate.ToUniversalTime(),
            EndDate = request.EndDate.ToUniversalTime(),
            TotalPrice = request.TotalPrice
        };

        var result = await _sender.Send(command);

        return Ok(result);
    }

    [HttpGet("all")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> GetAllReservations()
    {
        var result = await _sender.Send(new GetAllReservationsQuery());

        return Ok(result);
    }
}