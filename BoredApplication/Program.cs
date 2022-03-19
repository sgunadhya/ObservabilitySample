using System.Diagnostics;
using BoredClassLib;

void ConfigureActivitySource()
{
    Activity.DefaultIdFormat = ActivityIdFormat.W3C;
    Activity.ForceDefaultIdFormat = true;

    Console.WriteLine("         {0,-15} {1,-60} {2,-15}", "OperationName", "Id", "Duration");
    ActivitySource.AddActivityListener(new ActivityListener()
    {
        ShouldListenTo = (source) => true,
        Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
        ActivityStarted =
            activity => Console.WriteLine("Started: {0,-15} {1,-60}", activity.OperationName, activity.Id),
        ActivityStopped = activity => Console.WriteLine("Stopped: {0,-15} {1,-60} {2,-15}", activity.OperationName,
            activity.Id, activity.Duration)
    });
}

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<BoredService>();
        services.AddHttpClient();
        services.AddLogging();
        ConfigureActivitySource();
    })
    .Build();

await host.RunAsync();