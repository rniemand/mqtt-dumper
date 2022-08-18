using MqttDumper.Common.Extensions;
using MqttDumper.Common.Logging;
using MqttDumper.Common.Models;
using MqttDumper.Common.Services;
using MQTTnet;
using MQTTnet.Client;

namespace MqttDumperService;

public class Worker : BackgroundService
{
  private readonly ILoggerAdapter<Worker> _logger;
  private readonly IMqttMessageHandlerService _messageHandlerService;
  private readonly MqttDumperConfig _config;

  public Worker(ILoggerAdapter<Worker> logger,
    MqttDumperConfig config,
    IMqttMessageHandlerService messageHandlerService)
  {
    _logger = logger;
    _config = config;
    _messageHandlerService = messageHandlerService;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    var mqttFactory = new MqttFactory();
    using IMqttClient? mqttClient = mqttFactory.CreateMqttClient();

    mqttClient.ApplicationMessageReceivedAsync += _messageHandlerService.ProcessAsync;

    await mqttClient.ConnectAsync(_config.GetMqttClientOptions(), stoppingToken);

    var topicConfigs = _config.Subscriptions
      .Where(x => x.Enabled)
      .ToList();

    foreach (MqttDumperConfig.Subscription config in topicConfigs)
    {
      _logger.LogInformation("Subscribing to topic: {topic}", config.Topic);

      MqttClientSubscribeOptions? mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
        .WithTopicFilter(f => { f.WithTopic(config.Topic); })
        .Build();

      await mqttClient.SubscribeAsync(mqttSubscribeOptions, stoppingToken);
    }

    while (!stoppingToken.IsCancellationRequested)
    {
      await Task.Delay(1000, stoppingToken);
      await _messageHandlerService.TickAsync(stoppingToken);
    }
  }
}
