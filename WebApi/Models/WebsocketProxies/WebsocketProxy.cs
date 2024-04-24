using System.Diagnostics;

namespace WebApi.Models.WebsocketProxies;

/// <summary>
/// Represents a WebSocket proxy that manages a Node.js process to handle WebSocket connections.
/// </summary>
public class WebsocketProxy
{
    private Process process;
    private readonly string source;
    private readonly string target;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketProxy"/> class.
    /// </summary>
    /// <param name="source">The source address for the WebSocket proxy.</param>
    /// <param name="target">The target address to which WebSocket messages will be forwarded.</param>
    public WebsocketProxy(string source, string target)
    {
        this.source = source;
        this.target = target;
    }
    
    /// <summary>
    /// Starts the WebSocket proxy using a Node.js script.
    /// </summary>
    /// <param name="nodeExePath">The path to the Node.js executable.</param>
    /// <param name="scriptPath">The path to the Node.js script that implements the WebSocket proxy.</param>
    /// <returns>True if the WebSocket proxy was successfully started; otherwise, false.</returns>
    public bool Start(string nodeExePath, string scriptPath)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = nodeExePath,
            Arguments = $"{scriptPath} {source} {target}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        try
        {
            process = Process.Start(startInfo);
        }
        catch(Exception e)
        {
            return false;
        }
        

        process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
        process.ErrorDataReceived += (sender, e) => Console.WriteLine($"Error: {e.Data}");
        
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        return true;
    }

    /// <summary>
    /// Stops the WebSocket proxy process.
    /// </summary>
    /// <returns>True if the WebSocket proxy was successfully stopped or if no process is running; otherwise, false.</returns>
    public bool Stop()
    {
        if (process is null)
            return true;
        
        try
        {
            process.CancelErrorRead();
            process.CancelOutputRead();
            process.Kill();
            process.Close();
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}