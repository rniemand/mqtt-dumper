using MqttDumper.Common.Models;

namespace MqttDumperService;

public class Worker : BackgroundService
{
  private readonly ILogger<Worker> _logger;
  private readonly MqttDumperConfig _config;

  public Worker(ILogger<Worker> logger, MqttDumperConfig config)
  {
    _logger = logger;
    _config = config;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
      await Task.Delay(1000, stoppingToken);
    }
  }
}
