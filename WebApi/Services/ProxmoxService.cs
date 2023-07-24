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

    public async Task<IDictionary<string, object>> CreateNode(long id, string name)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu["101"].Clone
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

    public async Task<string> CreateNetInterface(long id, string name)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Network.CreateNetwork(id.ToString()+ name,"bridge",cidr:$"192.168.{id}.0/24");
            if (result.IsSuccessStatusCode)
            {
                return name;
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

    public async Task<IDictionary<string, object>> SetNetworkInterface(long id, string name)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[id.ToString()].Config.UpdateVmAsync(netN:new Dictionary<int,string>(){
                {0,name}
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
    public async Task<IDictionary<string, object>> SetRouterNetworkInterface(long id, string[] names)
    {
        try
        {
            var result = await proxmoxClient.Nodes[nodeName].Qemu[id.ToString()].Config.UpdateVmAsync(netN:new Dictionary<int,string>(){
                {0,names[0]},
                {1,names[1]},
                {2,names[2]},
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