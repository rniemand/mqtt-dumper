using System.Data;
using Dapper;
using MqttDumper.Common.Logging;
using MqttDumper.Common.Models;
using MySql.Data.MySqlClient;

namespace MqttDumper.Common.Repos;

public interface IRawMessagesRepo
{
  Task<int> AddAsync(ProcessedMqttMessage message);
}

public class RawMessagesRepo : IRawMessagesRepo
{
  private readonly ILoggerAdapter<RawMessagesRepo> _logger;
  private readonly MySqlConnection _connection;

  public RawMessagesRepo(ILoggerAdapter<RawMessagesRepo> logger, MqttDumperConfig config)
  {
    _logger = logger;

    _connection = new MySqlConnection(config.DbConnectionString);
  }


  // Public methods
  public async Task<int> AddAsync(ProcessedMqttMessage message)
  {
    ensureConnected();

    const string query = @"INSERT INTO `RawMessages`
    (
      `Retain`, `Dup`, `MessageExpiryInterval`, `TopicAlias`, `ParsedPayload`, `PayloadFormatIndicator`,
      `QualityOfServiceLevel`, `ResponseTopic`, `Topic`, `ContentType`, `ClientId`
    ) VALUES (
      @Retain, @Dup, @MessageExpiryInterval, @TopicAlias, @ParsedPayload, @PayloadFormatIndicator,
      @QualityOfServiceLevel, @ResponseTopic, @Topic, @ContentType, @ClientId
    )";

    return await _connection.ExecuteAsync(query, message);
  }


  // Internal methods
  private void ensureConnected()
  {
    // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
    switch (_connection.State)
    {
      case ConnectionState.Broken:
        _connection.Close();
        _connection.Open();
        return;
      case ConnectionState.Closed:
        _connection.Open();
        break;
    }
  }
}
