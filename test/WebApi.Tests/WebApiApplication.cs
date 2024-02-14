using System.Net.Http.Headers;
using Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApi.Tests;

public class WebApiApplication : WebApplicationFactory<Program>
{
    private const string Token = "asidj1082ednkasmsuh1928e2sc";
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Postgres"] = "",
                ["Authentication:LocalToken"] = Token,
            });
        });

        builder.ConfigureServices(services =>
        {
            var descriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PostgresDbContext>));

            if (descriptor != null) services.Remove(descriptor);
            services.AddDbContextFactory<PostgresDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        });
        return base.CreateHost(builder);
    }
    
    public new HttpClient CreateClient()
    {
        return CreateDefaultClient(new AuthHandler(req => { req.Headers.Authorization = new AuthenticationHeaderValue(Token); }));
    }
    
    public PostgresDbContext CreatePostgresDbContext()
    {
        var db = Services.GetRequiredService<IDbContextFactory<PostgresDbContext>>().CreateDbContext();
        db.Database.EnsureCreated();
        return db;
    }
    
    private sealed class AuthHandler(Action<HttpRequestMessage> onRequest) : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            onRequest(request);
            return base.SendAsync(request, cancellationToken);
        }
    }
}