using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers;

/// <summary>
/// Controller for managing virtual desktop operations.
/// </summary>
[Route("api/virtual-desktop")]
[ApiController]
public class VirtualDesktopController : ControllerBase
{
    private readonly VirtualDesktopService virtualDesktopService;

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualDesktopController"/> class.
    /// </summary>
    /// <param name="virtualDesktopService">The service responsible for virtual desktop operations.</param>
    public VirtualDesktopController(VirtualDesktopService virtualDesktopService)
    {
        this.virtualDesktopService = virtualDesktopService;
    }

    /// <summary>
    /// Starts a vm and its associated WebSocket proxy.
    /// </summary>
    /// <param name="userId">The identifier of the user starting virtual desktop.</param>
    /// <param name="labWorkId">The identifier of the lab work.</param>
    /// <returns>True if the virtual desktop was successfully started; otherwise, false.</returns>
    [HttpPost("start/{userId}/{labWorkId}", Name = nameof(StartVirtualDesktop))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> StartVirtualDesktop([FromRoute] string userId, [FromRoute]string labWorkId)
    {
        var res = await virtualDesktopService.StartVirtualDesktop(userId, labWorkId);

        return Ok(res);
    }

    /// <summary>
    /// Stops a vm and its associated WebSocket proxy.
    /// </summary>
    /// <param name="userId">The identifier of the user starting virtual desktop.</param>
    /// <returns>True if the virtual desktop was successfully stopped; otherwise, false.</returns>
    [HttpPost("stop/{userId}", Name = nameof(StopVirtualDesktop))]
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> StopVirtualDesktop([FromRoute] string userId)
    {
        var res = await virtualDesktopService.StopVirtualDesktop(userId);

        return Ok(res);
    }
}