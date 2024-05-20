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

    /// <summary>
    /// Creates a new vm record.
    /// </summary>
    /// <param name="vm">The vm record to create.</param>
    public async Task CreateAsync(Vm vm)
    {
        if (vm == null) throw new Exception();
        try
        {
            await vmCollection.InsertOneAsync(vm);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves a vm record by its ID.
    /// </summary>
    /// <param name="id">The ID of the vm to retrieve.</param>
    /// <returns>The vm record.</returns>
    public async Task<Vm> GetByIdAsync(string id)
    {
        try
        {
            return (await vmCollection.FindAsync(bson => bson.Id == id)).First();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    /// <summary>
    /// Updates an existing vm record.
    /// </summary>
    /// <param name="vm">The updated vm record.</param>
    public async Task UpdateAsync(Vm vm)
    {
        var vmToUpdate = await vmCollection.FindAsync(bson => bson.Id == vm.Id) ??
                              throw new Exception("Vm not found");
        try
        {
            var filter = Builders<Vm>.Filter.Eq("Id", vm.Id);
            var update = Builders<Vm>.Update
                .Set("VmId", vm.VmId)
                .Set("Name", vm.Name)
                .Set("LabId", vm.LabWorkId);
            await vmCollection.UpdateOneAsync(filter, update);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    /// <summary>
    /// Deletes a vm record by its ID.
    /// </summary>
    /// <param name="vmId">The ID of the laboratory work to delete.</param>
    public async Task DeleteAsync(string vmId)
    {
        var vm = await GetByIdAsync(vmId) ??
                      throw new Exception("Vm not found");
        try
        {
            await vmCollection.DeleteOneAsync(bson => bson.Id == vmId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    /// <summary>
    /// Retrieves all vm records.
    /// </summary>
    /// <returns>A list of all vm records.</returns>
    public async Task<List<Vm>> GetAllAsync()
    {
        try
        {
            return await vmCollection.Find(_ => true).ToListAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    /// <summary>
    /// Retrieves vm id in proxmox.
    /// </summary>
    /// <param name="id">The ID of the vm.</param>
    /// <returns>The vm id in proxmox</returns>
    public async Task<string> GetVmId(string id)
    {
        var vm = await GetByIdAsync(id);

        return vm.VmId.ToString();
    }
}
