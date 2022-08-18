using MqttDumper.Common.Services;

namespace MqttDumperService;

public class Worker : BackgroundService
{
  private readonly IMqttDumperService _dumperService;

  public Worker(IMqttDumperService dumperService)
  {
    _dumperService = dumperService;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    await _dumperService.SetupAsync(stoppingToken);

    while (!stoppingToken.IsCancellationRequested)
    {
      await Task.Delay(_dumperService.TickIntervalMs, stoppingToken);
      await _dumperService.TickAsync(stoppingToken);
    }
  }
}
