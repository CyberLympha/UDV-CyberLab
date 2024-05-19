using Corsinvest.ProxmoxVE.Api;

namespace WebApi.Services;

/// <summary>
/// Service for interacting with the Proxmox Virtual Environment through the Proxmox VE API.
/// </summary>
public class ProxmoxService
{
    private readonly IConfiguration configuration;
    private PveClient proxmoxClient;
    private string nodeName;
    private string kaliNode;
    private string winNode;
    private string ubuntuNode;

    /// <summary>
    /// Gets the host address of the Proxmox server.
    /// </summary>
    public string HostAddress { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProxmoxService"/> class.
    /// </summary>
    /// <param name="configuration">The application configuration where Proxmox credentials are stored.</param>
    public ProxmoxService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.nodeName = this.configuration["ProxmoxCredentials:NodeName"];
        this.kaliNode = this.configuration["ProxmoxCredentials:KaliNode"];
        this.winNode = this.configuration["ProxmoxCredentials:WinNode"];
        this.ubuntuNode = this.configuration["ProxmoxCredentials:UbuntuNode"];
        HostAddress = this.configuration["ProxmoxCredentials:Host"];
        proxmoxClient = new PveClient(this.configuration["ProxmoxCredentials:Host"]);
        var userName = this.configuration["ProxmoxCredentials:UserName"];
        var password = this.configuration["ProxmoxCredentials:Password"];
        proxmoxClient.Login(userName, password).Wait();
        proxmoxClient.ApiToken = this.configuration["ProxmoxCredentials:ApiKey"];
    }

    public async Task<IDictionary<string, object>> StartNode(string node)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[node].Status.Start.VmStart();
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception("Proxmox is unavailable");
        }
    }

    public async Task<IDictionary<string, object>> StopNode(string node)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[node].Status.Shutdown.VmShutdown(true);
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception("Proxmox is unavailable");
        }
    }

    public async Task<IDictionary<string, object>> VmStatus(string node)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[node].Status.Current.VmStatus();
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception("Proxmox is unavailable");
        }
    }

    public async Task<IDictionary<string, object>> SetVmPassword(string vmid, string username, string password,
        string sshKey)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[vmid].Config
                .UpdateVmAsync(ciuser: username, cipassword: password, sshkeys: sshKey.TrimEnd());
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception("Proxmox is unavailable");
        }
    }


    public async Task<IDictionary<string, object>> VmQemuConfig(string node)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[node].Config.VmConfig();
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception("Proxmox is unavailable");
        }
    }

    public async Task<IDictionary<string, object>> GetTaskStatus(string uuid)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Tasks[uuid].Status.ReadTaskStatus();
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception("Proxmox is unavailable");
        }
    }

    public async Task<IDictionary<string, object>> VmIp(string node)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[node].Agent.NetworkGetInterfaces
                .NetworkGetInterfaces();
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception("Proxmox is unavailable");
        }
    }

    public async Task<IDictionary<string, object>> CreateNode(long id, string name, string clonedVmId)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[clonedVmId].Clone
                .CloneVm((int)id, full: true, name: name);
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<string> CreateNetInterface(long id, string name, int subnet)
    {
        try
        {
            var iface = "vmbr" + id.ToString();
            var result = await proxmoxClient.Nodes[nodeName].Network.CreateNetwork(iface, type: "bridge",
                cidr: $"192.168.{subnet}.0/24", autostart: true);
            if (result.IsSuccessStatusCode)
            {
                return iface;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IDictionary<string, object>> ReloadNetworkInterface()
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Network.ReloadNetworkConfig();
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<string> SetNetworkInterface(long id, string name, int netN)
    {
        try
        {
            var vmConfig = (await proxmoxClient.Nodes[nodeName].Qemu[id.ToString()].Config.VmConfig())
                .ResponseToDictionary;
            var currentNetIface = ((IDictionary<string, object>)vmConfig["data"])[$"net{netN}"];

            string oldAddress = ((string)currentNetIface).Split(',')[0].Split('=')[1];
            string newAddress = $"rtl8139={oldAddress},bridge={name},firewall=1";

            var result = await proxmoxClient.Nodes[nodeName].Qemu[id.ToString()].Config.UpdateVmAsync(
                netN: new Dictionary<int, string>()
                {
                    { netN, newAddress }
                });

            if (result.IsSuccessStatusCode)
            {
                return newAddress;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IDictionary<string, object>> SetRouterNetworkInterface(long id, string[] names)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[id.ToString()].Config.UpdateVmAsync(
                netN: new Dictionary<int, string>()
                {
                    { 1, names[0] },
                    { 2, names[1] },
                    { 3, names[2] },
                });
            if (result.IsSuccessStatusCode)
            {
                return result.ResponseToDictionary;
            }

            throw new Exception(result.ReasonPhrase);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Creates a new virtual machine node by cloning an existing VM.
    /// </summary>
    /// <param name="id">The unique identifier for the new VM.</param>
    /// <param name="name">The name of the new VM.</param>
    /// <param name="clonedVmId">The VMID of the VM to clone.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
    public async Task<bool> CloneMachine(long id, string name, string clonedVmId)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[clonedVmId].Clone
                .CloneVm((int)id, full: true, name: name);

            return result.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    /// <summary>
    /// Checks if a virtual machine with the specified VMID exists in the Proxmox cluster.
    /// </summary>
    /// <param name="vmId">The VMID of the virtual machine to check.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a boolean result indicating if the VM exists.</returns>
    public async Task<bool> IsMachineExists(string vmId)
    {
        var res = await proxmoxClient.Cluster.Resources.Resources();
        var vmInfos = (IList<object>)res.Response.data;
        foreach (var vmInfo in vmInfos)
        {
            var vmInfoDict = (IDictionary<string, object>)vmInfo;
            if (vmInfoDict.TryGetValue("vmid", out var id) && id.ToString() == vmId)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Starts a virtual machine with the specified VMID.
    /// </summary>
    /// <param name="vmId">The VMID of the virtual machine to start.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
    public async Task<bool> StartMachine(string vmId)
    {
        if (await IsMachineRunning(vmId))
            return true;

        var vmRef = proxmoxClient.Nodes[nodeName].Qemu[vmId];
        var res = await vmRef.Status.Start.VmStart();

        return res.IsSuccessStatusCode;
    }

    /// <summary>
    /// Stops a virtual machine with the specified VMID.
    /// </summary>
    /// <param name="vmId">The VMID of the virtual machine to stop.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
    public async Task<bool> StopMachine(string vmId)
    {
        if (!await IsMachineRunning(vmId))
            return true;

        var vmRef = proxmoxClient.Nodes[nodeName].Qemu[vmId];

        return (await vmRef.Status.Suspend.VmSuspend()).IsSuccessStatusCode
               && (await vmRef.Status.Stop.VmStop()).IsSuccessStatusCode
               && (await vmRef.Status.Suspend.VmSuspend()).IsSuccessStatusCode;
    }

    /// <summary>
    /// Checks if a virtual machine with the specified VMID is currently running.
    /// </summary>
    /// <param name="vmId">The VMID of the virtual machine to check.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a boolean result indicating if the VM is running.</returns>
    public async Task<bool> IsMachineRunning(string vmId)
    {
        var vmRef = proxmoxClient.Nodes[nodeName].Qemu[vmId];
        var status = await vmRef.Status.Current.VmStatus();
        var vmStatus = status.Response.data.qmpstatus;

        return vmStatus == "running";
    }

    /// <summary>
    /// Checks if a virtual machine with the specified VMID is locked.
    /// </summary>
    /// <param name="vmId">The VMID of the virtual machine to check.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a boolean result indicating if the VM is locked.</returns>
    public async Task<bool> IsMachineLocked(string vmId)
    {
        var res = await proxmoxClient.Cluster.Resources.Resources();
        var vmInfos = (IList<object>)res.Response.data;
        foreach (var vmInfo in vmInfos)
        {
            var vmInfoDict = (IDictionary<string, object>)vmInfo;
            if (vmInfoDict.TryGetValue("vmid", out var id) && id.ToString() == vmId)
            {
                return vmInfoDict.TryGetValue("lock", out var lockReason);
            }
        }

        return false;
    }

    /// <summary>
    /// Adds a VNC parameter to the virtual machine configuration.
    /// </summary>
    /// <param name="vmId">The VMID of the virtual machine.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a boolean result indicating success or failure.</returns>
    public async Task<bool> EnableVnc(string vmId)
    {
        var apiToken = proxmoxClient.ApiToken;
        proxmoxClient.ApiToken = null;
        var vnc = $"-vnc 0.0.0.0:{vmId}";
        try
        {
            var res = await proxmoxClient.Nodes[nodeName].Qemu[vmId].Config.UpdateVmAsync(args: vnc);
            proxmoxClient.ApiToken = apiToken;

            return res.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            proxmoxClient.ApiToken = apiToken;

            return false;
        }
    }

    /// <summary>
    /// Checks if a VNC server is configured for the virtual machine.
    /// </summary>
    /// <param name="vmId">The VMID of the virtual machine.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation, with a boolean result indicating if VNC is configured.</returns>
    public async Task<bool> IsVncEnabled(string vmId)
    {
        var config = await proxmoxClient.Nodes[nodeName].Qemu[vmId].Config.VmConfig();
        var configDict = (IDictionary<string, object>)config.Response.data;
        if (!configDict.ContainsKey("args")) return false;
        var currentVncPort = ((string)configDict["args"]).Split(":")[1];

        return currentVncPort == vmId;
    }

    /// <summary>
    /// Retrieves the next available ID from the Proxmox cluster asynchronously.
    /// </summary>
    /// <returns>The next available ID as a string.</returns>
    /// <exception cref="Exception">Thrown when the request to Proxmox fails.</exception>
    public async Task<string> GetNextAvailableId()
    {
        var idResult = await proxmoxClient.Cluster.Nextid.Nextid();

        if (idResult.IsSuccessStatusCode)
        {
            return idResult.Response.data.ToString();
        }

        throw new Exception(idResult.ReasonPhrase);
    }

    /// <summary>
    /// Reads the content of a file from the virtual machine asynchronously.
    /// </summary>
    /// <param name="vmId">The identifier of the virtual machine.</param>
    /// <param name="filePath">The path to the file to be read.</param>
    /// <returns>The content of the file as a string if the operation is successful.</returns>
    /// <exception cref="Exception">Thrown when the file reading operation fails.</exception>
    public async Task<string> ReadFileAsync(string vmId, string filePath)
    {
        if (!await IsQemuGuestAgentEnabled(vmId))
            await EnableQemuGuestAgent(vmId);
        
        var fileContentResult = await proxmoxClient.Nodes[nodeName].Qemu[vmId].Agent.FileRead.FileRead(filePath);

        if (fileContentResult.IsSuccessStatusCode)
        {
            return fileContentResult.Response.data.content;
        }

        throw new Exception(fileContentResult.ReasonPhrase);
    }

    /// <summary>
    /// Clears the content of a file on a virtual machine.
    /// </summary>
    /// <param name="vmId">The ID of the virtual machine.</param>
    /// <param name="filePath">The path to the file to clear.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ClearFileContent(string vmId, string filePath)
    {
        if (!await IsQemuGuestAgentEnabled(vmId))
            await EnableQemuGuestAgent(vmId);
        await proxmoxClient.Nodes[nodeName].Qemu[vmId].Agent.Exec.Exec(new[] { "cp", "/dev/null", filePath });
    }

    /// <summary>
    /// Clears the content of multiple files on a virtual machine.
    /// </summary>
    /// <param name="vmId">The ID of the virtual machine.</param>
    /// <param name="filePaths">A list of paths to the files to clear.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ClearFilesContent(string vmId, List<string> filePaths)
    {
        foreach (var filePath in filePaths)
            await ClearFileContent(vmId, filePath);
    }
    
    /// <summary>
    /// Checks if the QEMU Guest Agent is enabled for the specified virtual machine.
    /// </summary>
    /// <param name="vmId">The identifier of the virtual machine.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a boolean value indicating whether the QEMU Guest Agent is enabled.
    /// </returns>
    /// <exception cref="Exception">Thrown when there is an error retrieving the VM configuration.</exception>
    public async Task<bool> IsQemuGuestAgentEnabled(string vmId)
    {
        var config = await proxmoxClient.Nodes[nodeName].Qemu[vmId].Config.VmConfig();
        var configDict = (IDictionary<string, object>)config.Response.data;
        if (!configDict.ContainsKey("agent")) return false;
        var agentParameter = (string)configDict["agent"];

        return agentParameter == "1";
    }
    
    /// <summary>
    /// Enables the QEMU Guest Agent for the specified virtual machine.
    /// </summary>
    /// <param name="vmId">The identifier of the virtual machine.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a boolean value indicating whether the QEMU Guest Agent was successfully enabled.
    /// </returns>
    /// <exception cref="Exception">Thrown when there is an error updating the VM configuration.</exception>
    public async Task<bool> EnableQemuGuestAgent(string vmId)
    {
        var apiToken = proxmoxClient.ApiToken;
        proxmoxClient.ApiToken = null;
        var agent = "1";
        try
        {
            var res = await proxmoxClient.Nodes[nodeName].Qemu[vmId].Config.UpdateVmAsync(agent: agent);
            proxmoxClient.ApiToken = apiToken;

            return res.IsSuccessStatusCode;
        }
        catch (Exception e)
        {
            proxmoxClient.ApiToken = apiToken;

            return false;
        }
    }
}