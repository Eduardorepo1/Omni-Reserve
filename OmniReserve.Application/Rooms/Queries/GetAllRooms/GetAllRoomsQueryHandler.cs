using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Application.Rooms.Queries.GetRoomById;
using System.Text.Json;

namespace OmniReserve.Application.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler
    : IRequestHandler<GetAllRoomsQuery, List<RoomResponseDto>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IDistributedCache _cache;

    private const string CACHE_KEY = "rooms_catalog";

    public GetAllRoomsQueryHandler(
        IRoomRepository roomRepository,
        IDistributedCache cache)
    {
        _roomRepository = roomRepository;
        _cache = cache;
    }

    public async Task<List<RoomResponseDto>> Handle(
        GetAllRoomsQuery request,
        CancellationToken cancellationToken)
    {
        // PASO A: Consultar Redis
        var cachedRooms = await _cache.GetStringAsync(
            CACHE_KEY,
            cancellationToken);

        // PASO B: Si existen datos en Redis, devolverlos
        if (!string.IsNullOrEmpty(cachedRooms))
        {
            var roomsFromCache =
                JsonSerializer.Deserialize<List<RoomResponseDto>>(cachedRooms);

            if (roomsFromCache is not null)
            {
                return roomsFromCache;
            }
        }

        // PASO C: Si Redis está vacío, consultar PostgreSQL
        var rooms = await _roomRepository.GetAllAsync();

        // Convertir las habitaciones a DTO
        var roomDtos = rooms.Select(room => new RoomResponseDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            Type = room.Type.ToString(),
            Price = room.PricePerNight,
            IsAvailable = room.IsAvailable
        }).ToList();

        // PASO D: Guardar los datos en Redis
        var serializedRooms = JsonSerializer.Serialize(roomDtos);

        // PASO E: Configurar expiración de 5 minutos
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        await _cache.SetStringAsync(
            CACHE_KEY,
            serializedRooms,
            cacheOptions,
            cancellationToken);

        return roomDtos;
    }
}