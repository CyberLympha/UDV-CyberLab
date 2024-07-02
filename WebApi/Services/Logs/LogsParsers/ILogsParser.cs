using WebApi.Models.Logs;

namespace WebApi.Services.Logs.LogsParsers;

/// <summary>
///     Interface for parsing logs of a specific type.
/// </summary>
public interface ILogsParser
{
    /// <summary>
    ///     Gets the type of logs that this parser can handle.
    /// </summary>
    public LogsType LogsType { get; }

    /// <summary>
    ///     Parses the provided logs text into a list of log entries.
    /// </summary>
    /// <param name="logsText">The text containing the logs to parse.</param>
    /// <returns>A list of log entries parsed from the logs text.</returns>
    List<Log> ParseLogs(string logsText);
}