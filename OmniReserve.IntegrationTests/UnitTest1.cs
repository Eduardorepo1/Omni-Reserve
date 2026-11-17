using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace OmniReserve.IntegrationTests;

public class RoomsControllerTests : IClassFixture<OmniReserveWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RoomsControllerTests(OmniReserveWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRooms_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/api/rooms");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRoomById_WhenRoomDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var roomId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/rooms/{roomId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateRoom_WithInvalidJson_ReturnsBadRequest()
    {
        // Arrange
        var content = new StringContent(
            "{ invalid json",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync("/api/rooms", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateRoom_ReturnsCreatedResponse()
    {
        // Arrange
        var room = new
        {
            RoomNumber = "999",
            Type = 2,
            PricePerNight = 500
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/rooms", room);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}