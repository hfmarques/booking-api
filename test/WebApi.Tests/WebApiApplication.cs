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
            var toRemove = services.Where(d => 
                d.ServiceType == typeof(DbContext) ||
                d.ServiceType == typeof(PostgresDbContext) ||
                (d.ServiceType.FullName != null && (d.ServiceType.FullName.Contains("PostgresDbContext") || d.ServiceType.FullName.Contains("DbContextOptions")))
            ).ToList();

            foreach (var descriptor in toRemove)
            {
                services.Remove(descriptor);
            }

            services.AddDbContextFactory<PostgresDbContext>(o => o.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            services.AddScoped<DbContext>(sp => sp.GetRequiredService<PostgresDbContext>());
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