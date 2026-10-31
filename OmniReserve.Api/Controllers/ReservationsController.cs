using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OmniReserve.Api.Requests;
using OmniReserve.Application.Reservations.Commands.CreateReservation;

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
}