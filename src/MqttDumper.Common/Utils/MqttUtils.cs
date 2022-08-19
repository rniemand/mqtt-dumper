using MqttDumper.Common.Models;
using MQTTnet.Client;
using System.Text;
using MqttDumper.Common.Logging;

namespace MqttDumper.Common.Utils;

public interface IMqttUtils
{
  ProcessedMqttMessage ProcessMessage(MqttApplicationMessageReceivedEventArgs message);
}

public class MqttUtils : IMqttUtils
{
  private readonly ILoggerAdapter<MqttUtils> _logger;

  public MqttUtils(ILoggerAdapter<MqttUtils> logger)
  {
    _logger = logger;
  }

  public ProcessedMqttMessage ProcessMessage(MqttApplicationMessageReceivedEventArgs message) =>
    new()
    {
      ParsedPayload = payloadToString(message),
      ClientId = message.ClientId,
      Payload = message.ApplicationMessage.Payload,
      PayloadFormatIndicator = message.ApplicationMessage.PayloadFormatIndicator.ToString("G"),
      MessageExpiryInterval = message.ApplicationMessage.MessageExpiryInterval,
      QualityOfServiceLevel = message.ApplicationMessage.QualityOfServiceLevel.ToString("G"),
      ResponseTopic = message.ApplicationMessage.ResponseTopic,
      Topic = message.ApplicationMessage.Topic,
      TopicAlias = message.ApplicationMessage.TopicAlias,
      ContentType = message.ApplicationMessage.ContentType,
      CorrelationData = message.ApplicationMessage.CorrelationData,
      Dup = message.ApplicationMessage.Dup,
      Retain = message.ApplicationMessage.Retain
    };

  private string payloadToString(MqttApplicationMessageReceivedEventArgs message)
  {
    try
    {
      return Encoding.Default.GetString(message.ApplicationMessage.Payload);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Failed to parse message payload");
      return string.Empty;
    }
  }
}
