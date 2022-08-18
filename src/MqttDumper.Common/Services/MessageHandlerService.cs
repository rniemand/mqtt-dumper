using MQTTnet.Client;
using System.Text.Json;

namespace MqttDumper.Common.Services;

public interface IMessageHandlerService
{
  Task ProcessAsync(MqttApplicationMessageReceivedEventArgs e);
  Task TickAsync(CancellationToken stoppingToken);
}

public class MessageHandlerService : IMessageHandlerService
{
  private bool _canProcessMessages;

  public async Task ProcessAsync(MqttApplicationMessageReceivedEventArgs e)
  {
    if (!_canProcessMessages)
      return;

    Console.WriteLine("Received application message.");
    e.DumpToConsole();

    await Task.CompletedTask;
  }

  public async Task TickAsync(CancellationToken stoppingToken)
  {
    if (!_canProcessMessages)
      _canProcessMessages = true;

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
