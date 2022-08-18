using MqttDumper.Common.Extensions;
using MqttDumperService;

IHost host = Host.CreateDefaultBuilder(args)
  .ConfigureServices(services =>
  {
    services
      .AddMqttDumper()
      .AddHostedService<Worker>();
  })
  .Build();

await host.RunAsync();
