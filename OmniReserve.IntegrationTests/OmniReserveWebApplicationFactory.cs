using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OmniReserve.Infrastructure.Persistence;

namespace OmniReserve.IntegrationTests;

public class OmniReserveWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Eliminar la configuración de PostgreSQL.
            var dbContextOptions = services.SingleOrDefault(
                descriptor =>
                    descriptor.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextOptions != null)
            {
                services.Remove(dbContextOptions);
            }

            // Eliminar configuraciones anteriores de EF Core.
            var optionsConfigurationDescriptors = services
                .Where(descriptor =>
                    descriptor.ServiceType.Name.Contains(
                        "IDbContextOptionsConfiguration"))
                .ToList();

            foreach (var descriptor in optionsConfigurationDescriptors)
            {
                services.Remove(descriptor);
            }

            // Crear una base de datos temporal para las pruebas.
            var dbName = Guid.NewGuid().ToString();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(dbName));

            // Mantener la autenticación falsa.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                    TestAuthHandler.AuthenticationScheme;

                options.DefaultChallengeScheme =
                    TestAuthHandler.AuthenticationScheme;

                options.DefaultScheme =
                    TestAuthHandler.AuthenticationScheme;

            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.AuthenticationScheme,
                options => { });

            // Sustituir Redis por caché en memoria.
            services.RemoveAll<IDistributedCache>();

            services.AddDistributedMemoryCache();
        });
    }
}