using Bl.Agrobook.Financial.Func.Options;
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

        services.Configure<FinancialApiOptions>(cfg =>
        {
            cfg.Login = ctx.Configuration[$"FinancialApi:Login"] ?? throw new ArgumentNullException("Login");
            cfg.Password = ctx.Configuration[$"FinancialApi:Password"] ?? throw new ArgumentNullException("Password");
            cfg.BaseUrl = ctx.Configuration[$"FinancialApi:BaseUrl"] ?? throw new ArgumentNullException("BaseUrl");
            cfg.AuthBaseUrl = ctx.Configuration[$"FinancialApi:AuthBaseUrl"] ?? throw new ArgumentNullException("BaseUrl");
        });
        services.Configure<AzureTableOption>(cfg =>
        {
            cfg.StorageKey = ctx.Configuration[$"AzureTableOption:StorageKey"] ?? throw new ArgumentNullException("StorageKey");
        });
        services.Configure<AuthOptions>(cfg =>
        {
            cfg.Key = ctx.Configuration[$"AuthOptions:Key"] ?? throw new ArgumentNullException("AuthOptions");
        });
        services.Configure<MongoDbOptions>(cfg =>
        {
            cfg.ConnectionString = ctx.Configuration[$"MongoDb:ConnectionString"] ?? throw new ArgumentNullException("MongoDb");
        });


        services.AddLogging()
            .AddMemoryCache()
            .AddHttpClient()

            .AddSingleton<Bl.Agrobook.Financial.Func.Repositories.ProductRepository>()

            .AddSingleton<Bl.Agrobook.Financial.Func.Services.AuthService>()
            .AddSingleton<Bl.Agrobook.Financial.Func.Services.AgrobookAuthRepository>()
            .AddSingleton<Bl.Agrobook.Financial.Func.Services.FinancialApiService>()
            .AddSingleton<Bl.Agrobook.Financial.Func.Services.CsvOrderReader>();
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
