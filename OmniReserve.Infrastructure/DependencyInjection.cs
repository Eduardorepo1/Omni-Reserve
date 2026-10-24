using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OmniReserve.Application.Common.Interfaces;
using OmniReserve.Infrastructure.Persistence;
using OmniReserve.Infrastructure.Persistence.Repositories;
using OmniReserve.Infrastructure.Security;

namespace OmniReserve.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("OmniReserveDb")));

        return services;
    }
}