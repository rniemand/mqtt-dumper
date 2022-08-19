using System.Diagnostics;
using YamlDotNet.Serialization;

namespace MqttDumper.Common.Models;

public class MqttDumperConfig
{
  [YamlMember(Alias = "server")]
  public ServerConfig Server { get; set; } = new();

  [YamlMember(Alias = "subscriptions")]
  public Subscription[] Subscriptions { get; set; } = Array.Empty<Subscription>();

  [YamlMember(Alias = "db_connection_string")]
  public string DbConnectionString { get; set; } = string.Empty;

  public class ServerConfig
  {
    [YamlMember(Alias = "host")]
    public string Host { get; set; } = "127.0.0.1";

    [YamlMember(Alias = "port")]
    public int Port { get; set; } = 1883;

    [YamlMember(Alias = "user")]
    public string Username { get; set; } = string.Empty;

    [YamlMember(Alias = "pass")]
    public string Password { get; set; } = string.Empty;

    public bool HasCredentials()
    {
      if (string.IsNullOrWhiteSpace(Username))
        return false;

      return !string.IsNullOrWhiteSpace(Password);
    }
  }

  [DebuggerDisplay("{Topic} ({Enabled})")]
  public class Subscription
  {
    [YamlMember(Alias = "topic")]
    public string Topic { get; set; } = string.Empty;

    [YamlMember(Alias = "enabled")]
    public bool Enabled { get; set; } = true;
  }
}
