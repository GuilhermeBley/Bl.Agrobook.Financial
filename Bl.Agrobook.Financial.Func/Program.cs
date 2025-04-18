using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices((ctx, services) =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddLogging()
            .AddMemoryCache()
            .AddHttpClient();
    })
    .ConfigureAppConfiguration(builder =>
    {
        builder
            .AddEnvironmentVariables()
            .AddJsonFile("local.settings.json", optional: true)
            .AddUserSecrets(typeof(Program).Assembly);
    })
    .Build();

await host.RunAsync();
