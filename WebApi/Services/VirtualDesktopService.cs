﻿using System.Reflection.Metadata;
using MongoDB.Bson;
using WebApi.Models;
using WebApi.Models.WebsocketProxies;

namespace WebApi.Services;

/// <summary>
/// Service responsible for managing virtual desktops and associated WebSocket proxies.
/// </summary>
public class VirtualDesktopService
{
    private readonly WebsocketProxyService websocketProxyService;
    private readonly ProxmoxService proxmoxService;
    private readonly LabWorkService labWorkService;
    private readonly UserService userService;
    private readonly VmService vmService;
    private readonly WebsocketProxySettings websocketProxySettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="VirtualDesktopService"/> class.
    /// </summary>
    /// <param name="websocketProxyService">The storage for WebSocket proxies.</param>
    /// <param name="proxmoxService">The service for interacting with Proxmox Virtual Environment.</param>
    /// <param name="websocketProxySettings">The settings for WebSocket proxies.</param>
    /// <param name="labWorkService">The service for interacting with LabWork records</param>
    /// <param name="userService">The service for interacting with User records</param>
    /// <param name="vmService">The service for interacting with Vm records</param>
    public VirtualDesktopService(
        WebsocketProxyService websocketProxyService,
        ProxmoxService proxmoxService,
        WebsocketProxySettings websocketProxySettings,
        LabWorkService labWorkService,
        UserService userService,
        VmService vmService)
    {
        this.websocketProxyService = websocketProxyService;
        this.proxmoxService = proxmoxService;
        this.websocketProxySettings = websocketProxySettings;
        this.labWorkService = labWorkService;
        this.userService = userService;
        this.vmService = vmService;
    }

    /// <summary>
    /// Starts a vm and its associated WebSocket proxy.
    /// </summary>
    /// <param name="userId">The identifier of the user starting virtual desktop.</param>
    /// <param name="labWorkId">The identifier of the lab work.</param>
    /// <returns>True if the virtual desktop was successfully started; otherwise, false.</returns>
    public async Task<bool> StartVirtualDesktop(string userId, string labWorkId)
    {
        var user = await userService.GetAsyncById(userId);
        var vmRecordId = user.VmId;
        if (vmRecordId is null || string.IsNullOrEmpty(vmRecordId))
        {
            if (!await CreateVmForUser(userId, labWorkId))
                return false;
            
            user = await userService.GetAsyncById(userId);
        }
        
        var vm = await vmService.GetByIdAsync(user.VmId);
        if (vm.LabWorkId != labWorkId)
        {
            await vmService.DeleteAsync(vm.Id);
            if (!await CreateVmForUser(userId, labWorkId))
                return false;
            user = await userService.GetAsyncById(userId);
            vm = await vmService.GetByIdAsync(user.VmId);
        }
        
        var vmId = vm.VmId.ToString();
        if (!await StartWebsocketProxy(vmId)) return false;
        if (await proxmoxService.StartMachine(vmId))
        {
            while (!await proxmoxService.IsMachineRunning(vmId))
            {
            }

            return true;
        }

        StopWebsocketProxy(vmId);

        return false;
    }

    /// <summary>
    /// Stops a vm and its associated WebSocket proxy.
    /// </summary>
    /// <param name="userId">The identifier of the user starting virtual desktop.</param>
    /// <returns>True if the virtual desktop was successfully stopped; otherwise, false.</returns>
    public async Task<bool> StopVirtualDesktop(string userId)
    {
        var user = await userService.GetAsyncById(userId);
        var vm = await vmService.GetByIdAsync(user.VmId);
        var vmId = vm.VmId.ToString();
        var hasStoppedVm = await proxmoxService.StopMachine(vmId);
        return StopWebsocketProxy(vmId) && hasStoppedVm;
    }

    private async Task<bool> StartWebsocketProxy(string vmId)
    {
        if (!await proxmoxService.IsVncExists(vmId))
            if (!await proxmoxService.AddVncParameter(vmId))
                return false;

        return websocketProxyService.Start(
            $"{websocketProxySettings.WebsocketHost}:{int.Parse(vmId)}",
            $"{proxmoxService.HostAddress}:{websocketProxySettings.ProxmoxVncStartingPort + int.Parse(vmId)}");
    }

    private bool StopWebsocketProxy(string vmId)
    {
        return websocketProxyService.Stop(
            $"{proxmoxService.HostAddress}:{websocketProxySettings.ProxmoxVncStartingPort + int.Parse(vmId)}");
    }

    private async Task<bool> CreateVmForUser(string userId, string labWorkId)
    {
        var user = await userService.GetAsyncById(userId);
        var newVmId = await proxmoxService.GetNextAvailableId();
        var labWork = await labWorkService.GetByIdAsync(labWorkId);
        var vmToCloneId = labWork.VmId;
        if (!await proxmoxService.CloneMachine(long.Parse(newVmId), newVmId, vmToCloneId))
            return false;

        while (await proxmoxService.IsMachineLocked(newVmId))
        {
        }

        var newVm = new Vm
        {
            LabWorkId = labWorkId,
            Name = newVmId,
            VmId = int.Parse(newVmId),
            Id = ObjectId.GenerateNewId().ToString()
        };
        await vmService.CreateAsync(newVm);

        user.VmId = newVm.Id;
        await userService.UpdateAsync(user);

        return true;
    }
}