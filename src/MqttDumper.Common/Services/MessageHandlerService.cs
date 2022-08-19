using MqttDumper.Common.Models;
using MQTTnet.Client;
using MqttDumper.Common.Logging;
using MqttDumper.Common.Processors;
using MqttDumper.Common.Repos;
using MqttDumper.Common.Utils;

namespace MqttDumper.Common.Services;

public interface IMessageHandlerService
{
  Task ProcessAsync(MqttApplicationMessageReceivedEventArgs message);
  Task TickAsync(CancellationToken stoppingToken);
  void RegisterTopicSubscription(MqttDumperConfig.Subscription config);
}

public class MessageHandlerService : IMessageHandlerService
{
  private readonly ILoggerAdapter<MessageHandlerService> _logger;
  private readonly List<IMessageProcessor> _processors = new();
  private readonly IMqttUtils _mqttUtils;
  private readonly IRawMessagesRepo _rawMessagesRepo;
  private bool _canProcessMessages=true;

  public MessageHandlerService(ILoggerAdapter<MessageHandlerService> logger,
    IMqttUtils mqttUtils,
    IRawMessagesRepo rawMessagesRepo)
  {
    _logger = logger;
    _mqttUtils = mqttUtils;
    _rawMessagesRepo = rawMessagesRepo;
  }

  // Public methods
  public async Task ProcessAsync(MqttApplicationMessageReceivedEventArgs message)
  {
    if (!_canProcessMessages)
      return;

    foreach (IMessageProcessor processor in _processors.Where(x => x.CanProcessMessage(message)))
      await runProcessorAsync(processor, message);
  }

  public async Task TickAsync(CancellationToken stoppingToken)
  {
    if (!_canProcessMessages)
      _canProcessMessages = true;

    await Task.CompletedTask;
  }

  public void RegisterTopicSubscription(MqttDumperConfig.Subscription config)
  {
    _logger.LogDebug("Registering message processor for topic: {topic}", config.Topic);
    _processors.Add(new MessageProcessor(config));
  }


  // Internal methods
  private async Task runProcessorAsync(IMessageProcessor processor, MqttApplicationMessageReceivedEventArgs message)
  {
    ProcessedMqttMessage processedMessage = _mqttUtils.ProcessMessage(message);


    await _rawMessagesRepo.AddAsync(processedMessage);

    
    await Task.CompletedTask;
  }
}
