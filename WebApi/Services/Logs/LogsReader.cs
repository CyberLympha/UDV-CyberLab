using WebApi.Models.Logs;
using WebApi.Services.Logs.LogsParsers;

namespace WebApi.Services.Logs;

/// <summary>
///     Service for reading logs using various parsers.
/// </summary>
public class LogsReader
{
    private readonly IEnumerable<ILogsParser> parsers;
    private readonly ProxmoxService proxmoxService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LogsReader" /> class.
    /// </summary>
    /// <param name="parsers">The collection of logs parsers to use.</param>
    /// <param name="proxmoxService">The Proxmox service for reading log files.</param>
    public LogsReader(IEnumerable<ILogsParser> parsers, ProxmoxService proxmoxService)
    {
        this.parsers = parsers;
        this.proxmoxService = proxmoxService;
    }

    private async Task<List<Log>> ReadLogs(LogsType logsType, string filePath, string vmId)
    {
        var parser = parsers.First(parser => parser.LogsType == logsType);
        var logs = await proxmoxService.ReadFileAsync(vmId, filePath);
        var parsedLogs = parser.ParseLogs(logs);

        return parsedLogs;
    }

    /// <summary>
    ///     Reads logs from multiple files for a virtual machine.
    /// </summary>
    /// <param name="filePaths">A dictionary mapping log types to their file paths.</param>
    /// <param name="vmId">The ID of the virtual machine.</param>
    /// <returns>A sorted list of combined log entries from all files.</returns>
    public async Task<List<Log>> ReadLogs(Dictionary<LogsType, string> filePaths, string vmId)
    {
        var combinedLogs = new List<Log>();
        foreach (var filePath in filePaths)
        {
            var readLogs = await ReadLogs(filePath.Key, filePath.Value, vmId);
            combinedLogs.AddRange(readLogs);
        }

        return combinedLogs.OrderBy(log => log.Time).ToList();
    }
}