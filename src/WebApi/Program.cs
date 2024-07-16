using Architecture.Ports;
using Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using WebApi.Apis.Customer;
using WebApi.Apis.Hotel;
using WebApi.Apis.Room;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureProjectSettings();

builder.ConfigureSerilog("webapi");

builder.ConfigureOpenTelemetry();

builder.ConfigureServicesFromProject();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<PostgresDbContext>();
if (!db.Database.IsInMemory())
{
// #if !DEBUG
    app.Logger.LogInformation("Initializing migrations...");
    await db.Database.MigrateAsync(); // apply the migrations
    app.Logger.LogInformation("Migrations Completed");
// #endif
}

app.MapHealthChecks("/health").AllowAnonymous();
app.MapHealthChecks("/alive", new HealthCheckOptions
{
    Predicate = r => r.Tags.Contains("live")
});

app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    app.Map("/", () => Results.Redirect("/swagger"));
}

app.AddApisFromHotels();
app.AddApisFromRooms();
app.AddApisFromCustomers();

app.Logger.LogInformation("WebApi Published...");

app.Run();

namespace WebApi
{
    public partial class Program
    {
    }
}