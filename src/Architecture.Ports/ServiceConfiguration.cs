using Architecture.Project.FileStorage;
using Architecture.Project.SqsMessageBus;
using Core;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Architecture.Ports;

public static class ServiceConfiguration
{
    public static void ConfigureServicesFromProject(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("Postgres");
        if (string.IsNullOrWhiteSpace(connectionString))
            connectionString = "";

        builder.Services.AddServicesFromData(connectionString);
        builder.Services.AddServicesFromCore();
        
        builder.Services.AddServicesFromFileStorage(
            builder.Environment.IsProduction(), 
            builder.Configuration["AWS:S3:ServiceUrl"] ?? "");

        var authenticationRegion = builder.Configuration["AWS:AuthenticationRegion"] ??
                                   throw new("Invalid AuthenticationRegion");
        if (builder.Environment.IsProduction())
        {
            var param = new MessageBusExtensionsParams()
                .WithAuthenticationRegion(authenticationRegion)
                .WithFallbackCredentialsFactory()
                .WithTransient();

            builder.Services.AddServicesFromMessageBus(param);
        }
        else
        {
            var sqsServiceUrl = builder.Configuration["AWS:Sqs:ServiceUrl"] ?? throw new("Invalid SqsServiceUrl");
            var param = new MessageBusExtensionsParams()
                .WithSqsServiceUrl(sqsServiceUrl)
                .WithAuthenticationRegion(authenticationRegion)
                .WithTransient();

            builder.Services.AddServicesFromMessageBus(param);
        }
        
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<PostgresDbContext>(
                "dbHealthCheck",
                customTestQuery: (db, cancellationToken) => db.Set<BookingStatus>().AnyAsync(cancellationToken)
            );
    }
}