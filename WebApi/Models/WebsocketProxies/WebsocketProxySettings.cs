using Microsoft.Build.Framework;

namespace WebApi.Models.WebsocketProxies;

/// <summary>
///     Represents settings for the WebSocket proxy.
/// </summary>
public record WebsocketProxySettings
{
    /// <summary>
    ///     Gets or sets the WebSocket host address.
    /// </summary>
    [Required]
    public required string WebsocketHost { get; set; }

    /// <summary>
    ///     Gets or sets the starting port for Proxmox VNC connections.
    /// </summary>
    [Required]
    public required int ProxmoxVncStartingPort { get; set; }

    /// <summary>
    ///     Gets or sets the websocket url path.
    /// </summary>
    [Required]
    public required string Path { get; set; }
}