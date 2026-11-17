using System.Net;

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
}