using System.Text.RegularExpressions;
using WebApi.Models;
using WebApi.Models.Logs;

namespace WebApi.Services.Logs.LogsParsers.TerminalLogsParser;

public class TerminalAuditLogsParser: ILogsParser
{
    private const string TimeRegexPattern = @"msg=audit\((\d+)";
    private const string ArgumentsRegexPattern =  @"argc=(\d+)";
    private const string ArgumentRegexPattern =  @"a\d+=""([^""]+)""";
    private const string PathPattern = @"cwd=""([^""]+)""";
    public LogsType LogsType { get; } = LogsType.Terminal;
    public List<Log> ParseLogs(string logsText)
    {
        var logs = logsText.Split("type=");
        var parsedLogs = new List<Log>(logs.Length);
        foreach (var log in logs)
        {
            if (log.Length > 6 && log.Substring(0, 6) == "EXECVE")
            {
                var time = ParseTime(log);
                var logArguments = ParseTerminalArguments(log);
                parsedLogs.Add(new Log(time, logArguments));
            }

            if (log.Length > 3 && log.Substring(0, 3) == "CWD")
            {
                var path = ParseLogPath(log);
                parsedLogs.Last().AddArgument(path);
            }
        }

        return parsedLogs;
    }

    private TimeSpan ParseTime(string log)
    {
        var match = Regex.Match(log, TimeRegexPattern);
        if (!match.Success)
            throw new Exception("log doesn't contain time");
        
        var seconds= match.Groups[1].Value;
        if(!int.TryParse(seconds, out var secondsAsNumber))
            throw new Exception("log doesn't contain time");
        
        return new TimeSpan(0,0, secondsAsNumber);
        
    }

    private List<string> ParseTerminalArguments(string log)
    {
        var arguments = new List<string>();
        var argcMatch = Regex.Match(log, ArgumentsRegexPattern);

        if (argcMatch.Success)
        {
            var argc = int.Parse(argcMatch.Groups[1].Value);
            for (var i = 0; i < argc; i++)
            {
                var argPatternWithIndex = ArgumentRegexPattern.Replace("a\\d+", $"a{i}");
                var argMatch = Regex.Match(log, argPatternWithIndex);

                if (argMatch.Success)
                    arguments.Add(argMatch.Groups[1].Value);
            }
        }

        return arguments;
    }

    private string ParseLogPath(string log)
    {
        var match = Regex.Match(log, PathPattern);
        if (!match.Success)
            throw new Exception();
        
        var path= match.Groups[1].Value;
        
        return path;
    }
}