using YamlDotNet.Serialization;

namespace MqttDumper.Common.Models;

public class MqttDumperConfig
{
  [YamlMember(Alias = "server")]
  public ServerConfig Server { get; set; } = new();

  public class ServerConfig
  {
    [YamlMember(Alias = "host")]
    public string Host { get; set; } = "127.0.0.1";

    [YamlMember(Alias = "port")]
    public int Port { get; set; } = 1883;
  }
}
