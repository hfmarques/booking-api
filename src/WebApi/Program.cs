using Core;
using Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

if (builder.Environment.IsDevelopment())
{
    builder.Host.UseSerilog((_, _, configuration) =>
    {
        configuration
            .MinimumLevel.Information()
            .WriteTo.Console()
            .Enrich.WithProperty("App", "booking-webapi")
            .Enrich.FromLogContext();
    });
}
else
{
    builder.Host.UseSerilog((context, _, configuration) =>
    {
        configuration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .WriteTo.Console()
            .Enrich.WithProperty("App", "booking-webapi")
            .Enrich.WithProperty("ContainerId", Environment.GetEnvironmentVariable("HOSTNAME") ?? string.Empty)
            .Enrich.WithProperty("Environment",
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty)
            .Enrich.FromLogContext();
    });
}

var connectionString = builder.Configuration.GetConnectionString("Postgres");
if (string.IsNullOrWhiteSpace(connectionString))
    connectionString = "";

builder.Services.AddServicesFromData(connectionString);
builder.Services.AddServicesFromCore();

builder.Services.AddHealthChecks();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { });

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// using var scope = app.Services.CreateScope();
// var db = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
// if (!db.Database.IsInMemory())
// {
//     app.Logger.LogInformation("Initializing migrations...");
//     await db.Database.MigrateAsync(); // apply the migrations
//     app.Logger.LogInformation("Migrations Completed");
// }

app.MapHealthChecks("/health").AllowAnonymous();

app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    app.Map("/", () => Results.Redirect("/swagger"));
}

app.Logger.LogInformation("WebApi Published...");

app.Run();

namespace WebApi
{
    public partial class Program
    {
    }
}