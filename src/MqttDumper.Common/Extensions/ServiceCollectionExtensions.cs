using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MqttDumper.Common.Logging;
using MqttDumper.Common.Models;
using NLog.Extensions.Logging;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace MqttDumper.Common.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddMqttDumper(this IServiceCollection services)
  {
    return services
      .AddLoggingAndConfig()
      .AddConfiguration();
  }

  private static IServiceCollection AddConfiguration(this IServiceCollection services)
  {
    var configFilePath = resolveConfigFilePath();

    var mqttDumperConfig = new DeserializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .IgnoreUnmatchedProperties()
      .Build()
      .Deserialize<MqttDumperConfig>(File.ReadAllText(configFilePath));

    return services.AddSingleton(mqttDumperConfig);
  }

  private static IServiceCollection AddLoggingAndConfig(this IServiceCollection services)
  {
    IConfigurationRoot? config = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: true)
      .Build();

    return services
      .AddLogging(loggingBuilder =>
      {
        // configure Logging with NLog
        loggingBuilder.ClearProviders();
        loggingBuilder.SetMinimumLevel(LogLevel.Trace);
        loggingBuilder.AddNLog(config);
      })
      .AddSingleton(typeof(ILoggerAdapter<>), typeof(LoggerAdapter<>))
      .AddSingleton<IConfiguration>(config);
  }

  private static string resolveConfigFilePath()
  {
    var path = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mqtt-dumper.yaml");

    if (!File.Exists(path))
      throw new FileNotFoundException("Unable to find configuration file!", path);

    var machineFile = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mqtt-dumper.machine.yaml");
    return File.Exists(machineFile) ? machineFile : path;
  }
}
