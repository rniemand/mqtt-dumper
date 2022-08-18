using Microsoft.Extensions.Logging;

namespace MqttDumper.Common.Logging;

public class LoggerAdapter<T> : ILoggerAdapter<T>
{
  private readonly ILogger<T> logger;

  public LoggerAdapter(ILogger<T> logger)
  {
    this.logger = logger;
  }

  // Trace
  public void LogTrace(string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Trace))
      return;

    logger.LogTrace(message, args);
  }

  public void LogTrace(Exception? exception, string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Trace))
      return;

    logger.LogTrace(exception, message, args);
  }

  // Debug
  public void LogDebug(string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Debug))
      return;

    logger.LogDebug(message, args);
  }

  public void LogDebug(Exception? exception, string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Debug))
      return;

    logger.LogDebug(exception, message, args);
  }

  // Information
  public void LogInformation(string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Information))
      return;

    logger.LogInformation(message, args);
  }

  public void LogInformation(Exception? exception, string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Information))
      return;

    logger.LogInformation(exception, message, args);
  }

  // Warning
  public void LogWarning(string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Warning))
      return;

    logger.LogWarning(message, args);
  }

  public void LogWarning(Exception? exception, string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Warning))
      return;

    logger.LogWarning(exception, message, args);
  }

  // Error
  public void LogError(string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Error))
      return;

    logger.LogError(message, args);
  }

  public void LogError(Exception? exception, string? message, params object?[] args)
  {
    if (!logger.IsEnabled(LogLevel.Error))
      return;

    logger.LogError(exception, message, args);
  }
}
