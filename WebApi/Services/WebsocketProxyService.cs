using System.Collections.Concurrent;
using WebApi.Models.WebsocketProxies;

namespace WebApi.Services;

/// <summary>
/// Service for managing WebSocket TCP proxies.
/// </summary>
public class WebsocketProxyService
{
    private readonly ConcurrentDictionary<int, WebSocketTcpProxy> proxies;

    /// <summary>
    /// Event raised when a WebSocket TCP proxy is stopped.
    /// </summary>
    public event EventHandler<int> WebSocketTcpProxyStopped;

    /// <summary>
    /// Initializes a new instance of the WebsocketProxyService class.
    /// </summary>
    public WebsocketProxyService()
    {
        proxies = new ConcurrentDictionary<int, WebSocketTcpProxy>();
    }

    /// <summary>
    /// Starts a WebSocket TCP proxy.
    /// </summary>
    /// <param name="webSocketPort">The port number for the WebSocket server.</param>
    /// <param name="tcpHost">The hostname of the TCP server.</param>
    /// <param name="tcpPort">The port number of the TCP server.</param>
    public void Start(int webSocketPort, string tcpHost, int tcpPort)
    {
        if (proxies.ContainsKey(webSocketPort))
            return;

        var proxy = new WebSocketTcpProxy(webSocketPort, tcpHost, tcpPort);
        proxy.ClientDisconnected += OnClientDisconnected;
        proxy.ExceptionExit += OnClientDisconnected;

        var cts = new CancellationTokenSource();
        proxy.Start(cts);
        proxies.TryAdd(webSocketPort, proxy);
    }

    /// <summary>
    /// Stops a WebSocket TCP proxy.
    /// </summary>
    /// <param name="webSocketPort">The port number of the WebSocket server.</param>
    public void Stop(int webSocketPort)
    {
        if (!proxies.TryRemove(webSocketPort, out var proxy)) return;

        proxy.ClientDisconnected -= OnClientDisconnected;
        proxy.ExceptionExit -= OnClientDisconnected;
        proxy.Stop();
        WebSocketTcpProxyStopped.Invoke(this, webSocketPort);
    }

    private void OnClientDisconnected(object? sender, int webSocketPort)
    {
        Console.WriteLine($"Client disconnected on WebSocket port: {webSocketPort}");
        Stop(webSocketPort);
    }
}