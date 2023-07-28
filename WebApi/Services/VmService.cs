using System.Diagnostics;
using System.Net;
using Corsinvest.ProxmoxVE.Api;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Models;


namespace WebApi.Services;

public class VmService
{
    private readonly IMongoCollection<User> usersCollection;
    private readonly IMongoCollection<Lab> labsCollection;
    private readonly IMongoCollection<LabEntity> labsEntityCollection;
    private readonly IMongoCollection<Vm> vmCollection;
    private readonly ProxmoxService proxmox;
    private readonly IConfiguration configuration;
    private string kaliNode;
    private string winNode;
    private string ubuntuNode;
    private string routerNode;

    public VmService(
        IMongoCollection<User> usersCollection, IConfiguration configuration, IMongoCollection<Vm> vmCollection,
        IMongoCollection<Lab> labsCollection, IMongoCollection<LabEntity> labsEntityCollection, ProxmoxService proxmox)
    {
        this.configuration = configuration;
        this.usersCollection = usersCollection;
        this.vmCollection = vmCollection;
        this.labsCollection = labsCollection;
        this.labsEntityCollection = labsEntityCollection;
        this.proxmox = proxmox;
        this.kaliNode = this.configuration["ProxmoxCredentials:KaliNode"];
        this.winNode = this.configuration["ProxmoxCredentials:WinNode"];
        this.ubuntuNode = this.configuration["ProxmoxCredentials:UbuntuNode"];
        this.routerNode = this.configuration["ProxmoxCredentials:RouterNode"];
    }


    public async Task CreateGroupVmAsync(long count)
    {
        var httpContext = new HttpContextAccessor().HttpContext;

        var userId = httpContext?.User.FindFirst(claim => claim.Type == "Id")?.Value;
	Console.WriteLine(userId);
        if (userId == null)
        {
            throw new Exception();
        }


        try
        {
            var resultKali = await proxmox.CreateNode(count + 1, "kali", this.kaliNode);

            var resultWin = await proxmox.CreateNode(count+2, "win", this.winNode);
            var resultUbuntu = await proxmox.CreateNode(count + 3, "ubuntu", this.ubuntuNode);
            var resultRouter = await proxmox.CreateNode(count + 4, "router",this.routerNode);

            var interKali = await proxmox.CreateNetInterface(count + 1,"kali", 2);
            var interWin = await proxmox.CreateNetInterface(count + 2,"win", 3);
            var interUbuntu = await proxmox.CreateNetInterface(count + 3,"ubuntu",4);

            await proxmox.ReloadNetworkInterface();
            await Task.Delay(620_000);
            var res1 = await proxmox.SetNetworkInterface(count + 1, interKali, 1);
            var res2 = await proxmox.SetNetworkInterface(count + 2, interWin, 0);
            var res3 = await proxmox.SetNetworkInterface(count + 3, interUbuntu, 1);
            await proxmox.SetRouterNetworkInterface(count + 4, new  string[]{res1, res2, res3});
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    public async Task<object> StartVm(string vmid)
    {
        try
        {
            var result = await proxmox.StartNode(vmid);
            return result["data"];
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IDictionary<string, object>> StopVm(int vmid)
    {
        try
        {
            var response = await proxmox.StopNode(vmid.ToString());
            return response;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    public async Task<IDictionary<string, object>> GetVmIp(string vmid)
    {
        try
        {
            var response = await proxmox.VmIp(vmid);
            return response;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IDictionary<string, object>> GetVmConfig(string vmid)
    {
        try
        {
            var response = await proxmox.VmQemuConfig(vmid);
            return response;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<dynamic> SetPassword(string vmid, string username, string password, string sshKey)
    {
        try
        {
            var response = await proxmox.SetVmPassword(vmid, username, password, sshKey);
            return response;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<IDictionary<string, object>> GetTask(string uuid)
    {
        try
        {
            var response = await proxmox.GetTaskStatus(uuid);
            return response;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task InsertMany(List<Vm> listVm)
    {
        await vmCollection.InsertManyAsync(listVm);
    }
}
