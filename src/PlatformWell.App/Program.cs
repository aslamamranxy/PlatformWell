using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformWell.App.Services;
using PlatformWell.App.Services.Startup;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        ServiceInitializer.RegisterServices(services, context.Configuration);

        services.AddTransient<Main>();
    })
    .Build();

var app = host.Services.GetRequiredService<Main>();
await app.RunAsync(); 