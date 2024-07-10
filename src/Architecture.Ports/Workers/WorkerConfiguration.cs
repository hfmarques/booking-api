using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Architecture.Ports.Workers;

public static class WorkerConfiguration
{
    public static void ConfigureServicesWorker(this IHostApplicationBuilder builder, WorkerWitness witness, string healthCheckName, short minutesToBeUnhealthy)
    {
        builder.Services.AddHealthChecks()
            .AddCheck(
                healthCheckName,
                () =>
                    DateTime.UtcNow.Subtract(witness.LastExecution).TotalMinutes < minutesToBeUnhealthy ?
                        HealthCheckResult.Healthy() :
                        HealthCheckResult.Unhealthy()
            );

        builder.Services.AddSingleton(_ => witness);
    }
}