using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OmniReserve.IntegrationTests;

public class OmniReserveWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Autenticación falsa para las pruebas
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
                options => { }
            );

            // Sustituir Redis por una caché en memoria
            services.RemoveAll<IDistributedCache>();

            services.AddDistributedMemoryCache();
        });
    }
}