namespace WebApi.Models.Logs;

/// <summary>
/// Represents a log entry with a timestamp and a list of arguments.
/// </summary>
public record Log(TimeSpan Time, List<string> Arguments)
{
    /// <summary>
    /// Adds an argument to the list of arguments.
    /// </summary>
    /// <param name="arg">The argument to add.</param>
    public void AddArgument(string arg)
    {
        Arguments.Add(arg);
    }
}

