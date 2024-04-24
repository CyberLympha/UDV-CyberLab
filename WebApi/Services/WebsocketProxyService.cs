using WebApi.Models.WebsocketProxies;

namespace WebApi.Services;

/// <summary>
/// Manages storage and lifecycle of WebSocket proxy instances.
/// </summary>
public class WebsocketProxyService
{
    private readonly Dictionary<string, WebsocketProxy> websocketProxies;
    private readonly string nodeExePath;
    private readonly string scriptPath;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="WebsocketProxyService"/> class.
    /// </summary>
    /// <param name="nodeExePath">The path to the Node.js executable.</param>
    /// <param name="scriptPath">The path to the Node.js script that implements the WebSocket proxy.</param>
    public WebsocketProxyService(string nodeExePath, string scriptPath)
    {
        this.nodeExePath = nodeExePath;
        this.scriptPath = scriptPath;
        websocketProxies = new Dictionary<string, WebsocketProxy>();
    }

    /// <summary>
    /// Starts a WebSocket proxy with the specified source and target addresses.
    /// </summary>
    /// <param name="source">The source address for the WebSocket proxy.</param>
    /// <param name="target">The target address to which WebSocket messages will be forwarded.</param>
    /// <returns>True if the WebSocket proxy was successfully started or is already running for the specified target; otherwise, false.</returns>
    public bool Start(string source, string target)
    {
        if (websocketProxies.ContainsKey(target))
            return true;
        
        var proxy = new WebsocketProxy(source, target);
        if (!proxy.Start(nodeExePath, scriptPath))
            return false;
        websocketProxies.Add(target, proxy);
        
        return true;
    }

    /// <summary>
    /// Stops the WebSocket proxy associated with the specified target address.
    /// </summary>
    /// <param name="target">The target address of the WebSocket proxy to stop.</param>
    /// <returns>True if the WebSocket proxy was successfully stopped or if no proxy exists for the specified target; otherwise, false.</returns>   
    public bool Stop(string target)
    {
        if (!websocketProxies.TryGetValue(target, out var proxy)) return false;
        if (!proxy.Stop())
            return false;
        websocketProxies.Remove(target);   
        
        return true;
    }
}