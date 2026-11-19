using NSubstitute;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Application.Reservations.Commands.CreateReservation;
using OmniReserve.Domain.Entities;

namespace OmniReserve.UnitTests;

public class CreateReservationCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenValidCommand_ReturnsNewReservationId()
    {
        // Arrange
        var reservationRepoMock = Substitute.For<IReservationRepository>();

        reservationRepoMock.IsRoomAvailableAsync(
            Arg.Any<Guid>(),
            Arg.Any<DateTime>(),
            Arg.Any<DateTime>())
            .Returns(Task.FromResult(true));

        var handler = new CreateReservationCommandHandler(
            reservationRepoMock);

        var command = new CreateReservationCommand
        {
            UserId = Guid.NewGuid(),
            RoomId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(3),
            TotalPrice = 1000m
        };

        // Act
        var result = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);

        await reservationRepoMock.Received(1).AddAsync(
            Arg.Any<Reservation>());
    }

    [Fact]
    public async Task Handle_WhenRoomIsNotAvailable_ThrowsException()
    {
        // Arrange
        var reservationRepoMock = Substitute.For<IReservationRepository>();

        reservationRepoMock.IsRoomAvailableAsync(
            Arg.Any<Guid>(),
            Arg.Any<DateTime>(),
            Arg.Any<DateTime>())
            .Returns(Task.FromResult(false));

        var handler = new CreateReservationCommandHandler(
            reservationRepoMock);

        var command = new CreateReservationCommand
        {
            UserId = Guid.NewGuid(),
            RoomId = Guid.NewGuid(),
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(3),
            TotalPrice = 1000m
        };

        // Act
        var exception = await Assert.ThrowsAsync<Exception>(
            () => handler.Handle(command, CancellationToken.None));

        // Assert
        Assert.Contains("habitación seleccionada ya está ocupada",
            exception.Message);

        await reservationRepoMock.DidNotReceive().AddAsync(
            Arg.Any<Reservation>());
    }
}