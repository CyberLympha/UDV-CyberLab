using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;

namespace WebApi.Models.WebsocketProxies;

/// <summary>
///     Represents a WebSocket TCP proxy server that forwards WebSocket traffic to a TCP server.
/// </summary>
public class WebSocketTcpProxy
{
    private readonly HttpListener httpListener;
    private readonly string tcpHost;
    private readonly int tcpPort;
    private readonly int webSocketPort;
    private CancellationTokenSource cancellationTokenSource;

    /// <summary>
    ///     Initializes a new instance of the WebSocketTcpProxy class.
    /// </summary>
    /// <param name="webSocketPort">The port number on which the WebSocket server listens.</param>
    /// <param name="tcpHost">The hostname of the TCP server to which WebSocket traffic will be forwarded.</param>
    /// <param name="tcpPort">The port number of the TCP server to which WebSocket traffic will be forwarded.</param>
    public WebSocketTcpProxy(int webSocketPort, string tcpHost, int tcpPort)
    {
        this.tcpHost = tcpHost;
        this.tcpPort = tcpPort;
        this.webSocketPort = webSocketPort;
        httpListener = new HttpListener();
        httpListener.Prefixes.Add($"http://*:{webSocketPort}/");
    }

    /// <summary>
    ///     Event raised when a client is disconnected from the WebSocket server.
    /// </summary>
    public event EventHandler<int> ClientDisconnected;

    /// <summary>
    ///     Event raised when an exception occurs and the server exits.
    /// </summary>
    public event EventHandler<int> ExceptionExit;

    /// <summary>
    ///     Starts the WebSocket TCP proxy server.
    /// </summary>
    /// <param name="cts">The CancellationTokenSource to cancel the operation.</param>
    public void Start(CancellationTokenSource cts)
    {
        cancellationTokenSource = cts;
        Task.Run(() => StartAsync(cts.Token));
    }

    /// <summary>
    ///     Stops the WebSocket TCP proxy server.
    /// </summary>
    public void Stop()
    {
        cancellationTokenSource.Cancel();
        if (httpListener is not null)
            httpListener.Stop();
    }

    private async Task StartAsync(CancellationToken cancellationToken)
    {
        StartHttpListener();
        await ListenForConnectionsAsync(cancellationToken);
    }

    private void StartHttpListener()
    {
        httpListener.Start();
        Console.WriteLine($"WebSocket server listening on port {webSocketPort}");
    }

    private async Task ListenForConnectionsAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            HttpListenerContext httpContext;
            try
            {
                httpContext = await httpListener.GetContextAsync();
            }
            catch (HttpListenerException) when (cancellationToken.IsCancellationRequested)
            {
                ExceptionExit.Invoke(this, webSocketPort);
                break;
            }

            if (httpContext.Request.IsWebSocketRequest)
            {
                var webSocketContext = await httpContext.AcceptWebSocketAsync(null);
                _ = HandleWebSocketConnectionAsync(webSocketContext.WebSocket, cancellationToken);
            }
            else
            {
                httpContext.Response.StatusCode = 400;
                httpContext.Response.Close();
            }
        }
    }

    private async Task HandleWebSocketConnectionAsync(WebSocket webSocket, CancellationToken cancellationToken)
    {
        using var tcpClient = new TcpClient();
        try
        {
            await tcpClient.ConnectAsync(tcpHost, tcpPort, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to TCP server: {ex}");
            await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Error connecting to TCP server",
                cancellationToken);
            return;
        }

        var networkStream = tcpClient.GetStream();
        var receiveTask = HandleTcpToWebSocketAsync(networkStream, webSocket, cancellationToken);
        var sendTask = HandleWebSocketToTcpAsync(webSocket, networkStream, cancellationToken);

        await Task.WhenAll(receiveTask, sendTask);

        await CleanupConnectionsAsync(webSocket, networkStream, tcpClient, cancellationToken);
    }

    private async Task HandleTcpToWebSocketAsync(NetworkStream networkStream, WebSocket webSocket,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[1024 * 1024];
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var size = await networkStream.ReadAsync(buffer, cancellationToken);
                if (size == 0) break;

                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, size), WebSocketMessageType.Binary,
                    true, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"WebSocket server stopped receiving from port {webSocketPort}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Receive error: {ex}");
        }
    }

    private async Task HandleWebSocketToTcpAsync(WebSocket webSocket, NetworkStream networkStream,
        CancellationToken cancellationToken)
    {
        var buffer = new byte[1024 * 1024];
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    ClientDisconnected.Invoke(this, webSocketPort);
                    break;
                }

                await networkStream.WriteAsync(buffer.AsMemory(0, result.Count), cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine($"WebSocket server stopped sending to port {webSocketPort}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Send error: {ex}");
        }
    }

    private async Task CleanupConnectionsAsync(WebSocket webSocket, NetworkStream networkStream, TcpClient tcpClient,
        CancellationToken cancellationToken)
    {
        networkStream.Close();
        tcpClient.Close();
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", cancellationToken);
    }
}