using AlertToTeams;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureAppConfiguration(builder =>
    {

    })
    .ConfigureServices(services =>
    {
        services.AddSingleton(provider => provider
                .GetRequiredService<IConfiguration>()
                .GetRequiredSection("Teams")
                .Get<TeamsSettings>() ?? throw new InvalidOperationException("Oops, no config"));
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
    })
    .Build();

host.Run();
