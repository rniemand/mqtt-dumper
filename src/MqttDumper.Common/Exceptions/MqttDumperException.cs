using System.Runtime.Serialization;

namespace MqttDumper.Common.Exceptions;

[Serializable]
public class MqttDumperException : Exception
{
  public MqttDumperException(string message)
    : base(message)
  { }

  protected MqttDumperException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
    if (info == null)
      throw new ArgumentNullException(nameof(info));
  }
}
