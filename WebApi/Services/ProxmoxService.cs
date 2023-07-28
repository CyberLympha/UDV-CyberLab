using Corsinvest.ProxmoxVE.Api;

namespace WebApi.Services;

public class ProxmoxService
{
    private readonly IConfiguration configuration;
    private PveClient proxmoxClient;
    private string nodeName;
    private string kaliNode;
    private string winNode;
    private string ubuntuNode;

    public ProxmoxService(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.nodeName = this.configuration["ProxmoxCredentials:NodeName"];
        this.kaliNode = this.configuration["ProxmoxCredentials:KaliNode"];
        this.winNode = this.configuration["ProxmoxCredentials:WinNode"];
        this.ubuntuNode = this.configuration["ProxmoxCredentials:UbuntuNode"];
        proxmoxClient = new PveClient(this.configuration["ProxmoxCredentials:Host"]);
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
            var result = await proxmoxClient.Nodes[nodeName].Qemu[node].Status.Shutdown.VmShutdown();
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
            var result = await proxmoxClient.Nodes[nodeName].Network.CreateNetwork(iface,type:"bridge",cidr:$"192.168.{subnet}.0/24",autostart: true);
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
            var vmConfig = (await proxmoxClient.Nodes[nodeName].Qemu[id.ToString()].Config.VmConfig()).ResponseToDictionary;
            var currentNetIface = ((IDictionary<string, object>)vmConfig["data"])[$"net{netN}"];

            string oldAddress=((string)currentNetIface).Split(',')[0].Split('=')[1];
            string newAddress = $"rtl8139={oldAddress},bridge={name},firewall=1";

            var result = await proxmoxClient.Nodes[nodeName].Qemu[id.ToString()].Config.UpdateVmAsync(netN:new Dictionary<int,string>(){
                {netN,newAddress}
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
            var result = await proxmoxClient.Nodes[nodeName].Qemu[id.ToString()].Config.UpdateVmAsync(netN:new Dictionary<int,string>(){
                {1,names[0]},
                {2,names[1]},
                {3,names[2]},
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
}