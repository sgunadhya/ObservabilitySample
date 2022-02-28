using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BoredClassLib;

public class BoredService : IHostedService
{
    private const string EndPoint = "https://www.boredapi.com/api/activity";

    private ActivitySource _source = new("BoredAPITracer");

    static Meter _meter = new("BoredAPIMeter");

    static Histogram<long> _latency = _meter.CreateHistogram<long>("BoredAPILatency");

    private ILogger<BoredService> _logger;

    private IHttpClientFactory _factory;

    public BoredService(ILogger<BoredService> logger, IHttpClientFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (_source.CreateActivity("Calling BoredAPI", ActivityKind.Client))
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            _logger.LogInformation("Starting BoredService");
            HttpClient httpClient = _factory.CreateClient();
            HttpResponseMessage message = await httpClient.GetAsync(EndPoint);
            timer.Stop();
            _latency.Record(timer.ElapsedMilliseconds);
            string response = await message.Content.ReadAsStringAsync();
            _logger.LogDebug($"Got message from API : {response}");
            Bored? bored = JsonSerializer.Deserialize<Bored>(response);
            Console.WriteLine($"You can try this: {bored?.Activity} \n" +
                              $"which is an activity of type : {bored?.Type}, \n" +
                              $"and needs {bored?.Participants} people. \n " +
                              $"This will cost you : {bored?.Price} ");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping BoredService");
        return Task.CompletedTask;
    }
}