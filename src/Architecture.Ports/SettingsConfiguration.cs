using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Architecture.Ports;

public static class SettingsConfiguration
{
    public static void ConfigureProjectSettings(this IHostApplicationBuilder builder)
    {
        var settingsPath = Path.Combine(AppContext.BaseDirectory, "appsettings.global.json");
        
        builder.Configuration.AddJsonFile(settingsPath, optional: false, reloadOnChange: true);
        
        builder.Configuration.AddJsonFile("appsettings.json", optional: false);
        
        if (builder.Environment.IsDevelopment())
            builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
        
        builder.Configuration
            .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
            .AddEnvironmentVariables();
    }
}