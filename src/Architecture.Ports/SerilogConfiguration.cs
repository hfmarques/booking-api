using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.OpenTelemetry;

namespace Architecture.Ports;

public static class SerilogConfiguration
{
    public static void ConfigureSerilog(this IHostApplicationBuilder builder, string appName)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Logging.ClearProviders();

        builder.Services.AddSerilog(config =>
        {
            config.MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithProcessName()
                .Enrich.WithThreadId()
                .Enrich.WithSpan()
                .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
                    .WithDefaultDestructurers()
                    .WithDestructurers(new[] {new DbUpdateExceptionDestructurer()}))
                .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
                .Enrich.WithProperty("App", appName)
                .Enrich.WithProperty("ContainerId", Environment.GetEnvironmentVariable("HOSTNAME") ?? string.Empty)
                .Enrich.WithProperty("DotnetEnvironment", Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? string.Empty)
                .Enrich.FromLogContext()
                .WriteTo.Console();

            if (builder.Environment.IsProduction())
                config
                    .MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
                    .Filter.ByExcluding(x =>
                        x.Properties.Any(p => p.Value.ToString().Contains("swagger")));
            else
            {
                var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
                if (useOtlpExporter)
                {
                    config.WriteTo.OpenTelemetry(options =>
                    {
                        options.IncludedData = IncludedData.TraceIdField | IncludedData.SpanIdField;
                        options.Endpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? string.Empty;
                        AddHeaders(options.Headers, builder.Configuration["OTEL_EXPORTER_OTLP_HEADERS"] ?? string.Empty);
                        AddResourceAttributes(options.ResourceAttributes, builder.Configuration["OTEL_RESOURCE_ATTRIBUTES"] ?? string.Empty);
                        return;

                        void AddHeaders(IDictionary<string, string> headers, string headerConfig)
                        {
                            if (string.IsNullOrEmpty(headerConfig)) return;

                            foreach (var header in headerConfig.Split(','))
                            {
                                var parts = header.Split('=');

                                if (parts.Length == 2)
                                {
                                    headers[parts[0]] = parts[1];
                                }
                                else
                                {
                                    throw new InvalidOperationException($"Invalid header format: {header}");
                                }
                            }
                        }

                        void AddResourceAttributes(IDictionary<string, object> attributes, string attributeConfig)
                        {
                            if (string.IsNullOrEmpty(attributeConfig)) return;

                            var parts = attributeConfig.Split('=');

                            if (parts.Length == 2)
                            {
                                attributes[parts[0]] = parts[1];
                            }
                            else
                            {
                                throw new InvalidOperationException($"Invalid resource attribute format: {attributeConfig}");
                            }
                        }
                    });
                }
            }
        });
    }
}