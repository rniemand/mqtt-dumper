namespace MqttDumper.Common.Models;

public class ProcessedMqttMessage
{
  public string ClientId { get; set; } = string.Empty;
  public string ParsedPayload { get; set; } = string.Empty;
  public byte[] Payload { get; set; } = Array.Empty<byte>();
  public string PayloadFormatIndicator { get; set; } = string.Empty;
  public uint MessageExpiryInterval { get; set; }
  public string QualityOfServiceLevel { get; set; } = string.Empty;
  public string ResponseTopic { get; set; } = string.Empty;
  public string Topic { get; set; } = string.Empty;
  public ushort TopicAlias { get; set; }
  public string ContentType { get; set; } = string.Empty;
  public byte[] CorrelationData { get; set; } = Array.Empty<byte>();
  public bool Dup { get; set; }
  public bool Retain { get; set; }
}
