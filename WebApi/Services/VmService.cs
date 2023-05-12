using System.Diagnostics;
using System.Net;
using Corsinvest.ProxmoxVE.Api;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Models;


namespace WebApi.Services;

public class VmService
{
    private readonly IMongoCollection<User> _usersCollection;
    private readonly IConfiguration _configuration;
    private PveClient proxmoxClient;
    private string nodeName;
    private string winNode;
    private string ubuntuNode;
    private string kaliNode;

    public VmService(
        IMongoCollection<User> usersCollection, IConfiguration configuration)
    {
        _configuration = configuration;
        _usersCollection = usersCollection;
        nodeName = _configuration["ProxmoxCredentials:NodeName"];
        kaliNode = _configuration["ProxmoxCredentials:KaliNode"];
        ubuntuNode = _configuration["ProxmoxCredentials:UbuntuNode"];
        winNode = _configuration["ProxmoxCredentials:WinNode"];
        proxmoxClient = new PveClient(_configuration["ProxmoxCredentials:Host"]);
        proxmoxClient.ApiToken = _configuration["ProxmoxCredentials:ApiKey"];
    }

    public async Task<Result> GetVmsAsync(int vmid, string name)
    {
        return await proxmoxClient.Nodes[nodeName].Qemu["9000"].Clone.CloneVm(vmid, full: true, name: name);
    }

    public async Task<Result> CreateVmAsync(Vm vm, VmType type)
    {
        var httpContext = new HttpContextAccessor().HttpContext;
        var vmType = kaliNode;
        if (type == VmType.Windows)
        {
            vmType = winNode;
        }
        else if(type == VmType.Ubuntu)
        {
            vmType = ubuntuNode;
        }
        var result = await proxmoxClient.Nodes[nodeName].Qemu[vmType].Clone.CloneVm(vm.Vmid, full: true, name: vm.Name);
        
        if (result.IsSuccessStatusCode)
        {
            var userId = httpContext?.User.FindFirst(claim => claim.Type == "Id")?.Value;

            if (userId == null)
            {
                throw new Exception();
            }

            var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(userId));
            var user = (await _usersCollection.FindAsync(filter)).FirstOrDefault();
            if (user != null)
            {
                if (user.Vms.Length > 0)
                {
                    var newVms = user.Vms.Append(vm.Vmid).ToArray();
                    var update = Builders<User>.Update.Set("Vms", newVms);
                    await _usersCollection.FindOneAndUpdateAsync(filter, update);
                }
                else
                {
                    var update = Builders<User>.Update.Set("Vms", new int[] { vm.Vmid });
                    await _usersCollection.FindOneAndUpdateAsync(filter, update);
                }

                return result;
            }
        }

        return result;
    }

    public async Task<Result> StartVm(int vmid)
    {
        var response = await proxmoxClient.Nodes[nodeName].Qemu[$"{vmid}"].Status.Start.VmStart();
        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        return null;
    }

    public async Task<Result> StopVm(int vmid)
    {
        var response = await proxmoxClient.Nodes[nodeName].Qemu[$"{vmid}"].Status.Shutdown.VmShutdown();
        if (response.IsSuccessStatusCode)
        {
            return response.Response;
        }

        return null;
    }

    public async Task<IDictionary<string, object>> GetStatus(int vmid)
    {
        var response = await proxmoxClient.Nodes[nodeName].Qemu[$"{vmid}"].Status.Current.VmStatus();
        if (response.IsSuccessStatusCode)
        {
            return response.ResponseToDictionary;
        }

        return null;
    }

    public async Task<dynamic> SetPassword(int vmid, string username, string password, string sshKey)
    {
        var response = await proxmoxClient.Nodes[nodeName].Qemu[$"{vmid}"].Config
            .UpdateVmAsync(ciuser: username, cipassword: password, sshkeys: sshKey.TrimEnd());
        if (response.IsSuccessStatusCode)
        {
            return response.Response;
        }

        return null;
    }
}