using MqttDumper.Common.Models;
using MQTTnet.Client;
using System.Text.Json;
using MqttDumper.Common.Logging;
using MqttDumper.Common.Processors;

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
  private bool _canProcessMessages;

  public MessageHandlerService(ILoggerAdapter<MessageHandlerService> logger)
  {
    _logger = logger;
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
    Console.WriteLine("Received application message.");
    message.DumpToConsole();

    await Task.CompletedTask;
  }
}


internal static class ObjectExtensions
{
  public static TObject DumpToConsole<TObject>(this TObject @object)
  {
    var output = "NULL";
    if (@object != null)
    {
      output = JsonSerializer.Serialize(@object, new JsonSerializerOptions
      {
        WriteIndented = true
      });
    }

    Console.WriteLine($"[{@object?.GetType().Name}]:\r\n{output}");
    return @object;
  }
}
