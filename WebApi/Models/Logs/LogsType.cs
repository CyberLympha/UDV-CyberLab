namespace WebApi.Models.Logs;

/// <summary>
/// Enumerates the types of logs associated with a laboratory work instruction.
/// </summary>
public enum LogsType
{
    /// <summary>
    /// Terminal logs.
    /// </summary>
    Terminal,
    /// <summary>
    /// Stall until other types of logs other than terminal are created.
    /// </summary>
    Empty
}