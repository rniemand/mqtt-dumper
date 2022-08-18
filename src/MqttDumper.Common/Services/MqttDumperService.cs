using MqttDumper.Common.Extensions;
using MqttDumper.Common.Logging;
using MqttDumper.Common.Models;
using MQTTnet;
using MQTTnet.Client;

namespace MqttDumper.Common.Services;

public interface IMqttDumperService
{
  int TickIntervalMs { get; }
  Task SetupAsync(CancellationToken stoppingToken);
  Task TickAsync(CancellationToken stoppingToken);
}

public class MqttDumperService : IMqttDumperService
{
  public const int DEFAULT_TICK_INTERVAL_MS = 1500;
  public int TickIntervalMs { get; internal set; } = DEFAULT_TICK_INTERVAL_MS;

  private readonly ILoggerAdapter<MqttDumperService> _logger;
  private readonly MqttDumperConfig _config;
  private readonly IMqttClient _mqttClient;
  private readonly MqttFactory _mqttFactory;
  private readonly IMessageHandlerService _messageHandlerService;

  public MqttDumperService(ILoggerAdapter<MqttDumperService> logger,
    MqttDumperConfig config,
    IMessageHandlerService messageHandlerService)
  {
    _logger = logger;
    _config = config;
    _messageHandlerService = messageHandlerService;

    _mqttFactory = new MqttFactory();
    _mqttClient = _mqttFactory.CreateMqttClient();

    _mqttClient.ApplicationMessageReceivedAsync += messageHandlerService.ProcessAsync;
  }

  public async Task SetupAsync(CancellationToken stoppingToken)
  {
    await _mqttClient.ConnectAsync(_config.GetMqttClientOptions(), stoppingToken);

    var topicConfigs = _config.Subscriptions
      .Where(x => x.Enabled)
      .ToList();

    foreach (MqttDumperConfig.Subscription config in topicConfigs)
    {
      _logger.LogInformation("Subscribing to topic: {topic}", config.Topic);

      MqttClientSubscribeOptions? mqttSubscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
        .WithTopicFilter(f => { f.WithTopic(config.Topic); })
        .Build();

      await _mqttClient.SubscribeAsync(mqttSubscribeOptions, stoppingToken);
    }
  }

  public async Task TickAsync(CancellationToken stoppingToken)
  {
    if (TickIntervalMs == DEFAULT_TICK_INTERVAL_MS)
      TickIntervalMs = 500;

    await _messageHandlerService.TickAsync(stoppingToken);
  }
}
