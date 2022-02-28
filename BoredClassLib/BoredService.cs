using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BoredClassLib;

public class BoredService : IHostedService
{
    private const string EndPoint = "https://www.boredapi.com/api/activity";
    
    private ILogger<BoredService> _logger;

    private IHttpClientFactory _factory;

    public BoredService(ILogger<BoredService> logger, IHttpClientFactory factory)
    {
        _logger = logger;
        _factory = factory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting BoredService");
        HttpClient httpClient = _factory.CreateClient();
        HttpResponseMessage message = await httpClient.GetAsync(EndPoint);
        string response = await message.Content.ReadAsStringAsync();
        _logger.LogDebug($"Got message from API : {response}");
        Bored? bored = JsonSerializer.Deserialize<Bored>(response);
        Console.WriteLine($"You can try this: {bored?.Activity} \n" +
                          $"which is an activity of type : {bored?.Type}, \n" +
                          $"and needs {bored?.Participants} people. \n " +
                          $"This will cost you : {bored?.Price} ");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping BoredService");
        return Task.CompletedTask;
    }
}