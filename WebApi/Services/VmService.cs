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
    }


    public async Task<bool> CreateGroupVmAsync(long id)
    {
        var httpContext = new HttpContextAccessor().HttpContext;

        var userId = httpContext?.User.FindFirst(claim => claim.Type == "Id")?.Value;

        if (userId == null)
        {
            throw new Exception();
        }

        var filter = Builders<User>.Filter.Eq("_id", ObjectId.Parse(userId));
        // var user = (await _usersCollection.FindAsync(filter)).FirstOrDefault();
        try
        {
            var resultKali = await proxmox.CreateNode(id + 1, "kali");
            var resultWin = await proxmox.CreateNode(id + 2, "win");
            var resultUbuntu = await proxmox.CreateNode(id + 3, "ubuntu");
            return true;
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

    public async Task InsertMany(Vm firstVm, Vm secondVm, Vm thirdVm)
    {
        await vmCollection.InsertManyAsync(new List<Vm> { firstVm, secondVm, thirdVm });
    }
}