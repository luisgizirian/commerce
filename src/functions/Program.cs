using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;

var host = new HostBuilder()
    
    .ConfigureFunctionsWorkerDefaults()
    
    //.UseEnvironment(EnvironmentName.Development)
    .ConfigureAppConfiguration((configBuilder) => configBuilder.AddEnvironmentVariables())
    .ConfigureServices((hostContext, services) =>
    {
        // services.AddHttpClient(Constants.SWIO_UTILITIES, client =>
        // {
        //     client.BaseAddress = new Uri(hostContext.Configuration["Utilities"]);
        //     client.DefaultRequestHeaders.Add("x-functions-key", hostContext.Configuration["TenantConfigKey"]);
        // });

        // services.AddScoped<IMailEnqueuer, MailEnqueuer>();
    })
    .ConfigureLogging((context, logging) =>
    {
    })
    .UseConsoleLifetime()

    .Build();

host.Run();
