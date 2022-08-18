using MqttDumper.Common.Models;
using MQTTnet.Client;

namespace MqttDumper.Common.Extensions;

public static class MqttExtensions
{
  public static MqttClientOptions GetMqttClientOptions(this MqttDumperConfig config)
  {
    MqttClientOptionsBuilder? builder = new MqttClientOptionsBuilder()
      .WithTcpServer(config.Server.Host, config.Server.Port);

    if (config.Server.HasCredentials())
      builder.WithCredentials(config.Server.Username, config.Server.Password);

    return builder.Build();
  }
}
