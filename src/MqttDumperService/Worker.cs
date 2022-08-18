using System.Text.Json;
using MqttDumper.Common.Extensions;
using MqttDumper.Common.Models;
using MQTTnet;
using MQTTnet.Client;

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

    var mqttFactory = new MqttFactory();

    using (IMqttClient? mqttClient = mqttFactory.CreateMqttClient())
    {
      MqttClientOptions mqttClientOptions = _config.GetMqttClientOptions();

      // Setup message handling before connecting so that queued messages
      // are also handled properly. When there is no event handler attached all
      // received messages get lost.
      mqttClient.ApplicationMessageReceivedAsync += e =>
      {
        Console.WriteLine("Received application message.");
        e.DumpToConsole();

        return Task.CompletedTask;
      };

      await mqttClient.ConnectAsync(mqttClientOptions, stoppingToken);

      MqttClientSubscribeOptions? mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
        .WithTopicFilter(f => { f.WithTopic("#"); })
        .Build();

      await mqttClient.SubscribeAsync(mqttSubscribeOptions, stoppingToken);

      Console.WriteLine("MQTT client subscribed to topic.");
      Console.WriteLine("Press enter to exit.");
      Console.ReadLine();
    }



    while (!stoppingToken.IsCancellationRequested)
    {
      _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
      await Task.Delay(1000, stoppingToken);
    }
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
