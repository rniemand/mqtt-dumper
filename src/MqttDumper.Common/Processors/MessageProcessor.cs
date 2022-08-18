using MqttDumper.Common.Models;
using MQTTnet.Client;

namespace MqttDumper.Common.Processors;

public interface IMessageProcessor
{
  bool CanProcessMessage(MqttApplicationMessageReceivedEventArgs message);
}

public class MessageProcessor : IMessageProcessor
{
  private readonly bool _processAllMessages;
  private readonly string _safeTopic;

  public MessageProcessor(MqttDumperConfig.Subscription config)
  {
    _safeTopic = config.Topic;

    if (_safeTopic == "#")
      _processAllMessages = true;

    if (_safeTopic.EndsWith("#"))
      _safeTopic = _safeTopic.Split("#")[0];
  }

  public bool CanProcessMessage(MqttApplicationMessageReceivedEventArgs message) =>
    _processAllMessages || message.ApplicationMessage.Topic.StartsWith(_safeTopic);
}
