using BoredClassLib;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<BoredService>();
        services.AddHttpClient();
        services.AddLogging();
    })
    .Build();

await host.RunAsync();