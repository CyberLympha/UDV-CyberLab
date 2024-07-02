using Corsinvest.ProxmoxVE.Api;
using MongoDB.Bson;
using MongoDB.Driver;
using WebApi.Model.AuthModels;
using WebApi.Model.LabModels;
using WebApi.Model.VirtualMachineModels;

namespace WebApi.Services;

public class LabsService
{
    private readonly IMongoCollection<Lab> _labsCollection;
    private readonly IMongoCollection<LabEntity> _labsEntityCollection;
    private readonly IMongoCollection<Vm> _vmsCollection;
    private readonly VmService _vmService;
    private readonly UserService _userService;
    private readonly ProxmoxService proxmoxService;

    public LabsService(
        IMongoCollection<Lab> labsCollection, IMongoCollection<Vm> vmsCollection, VmService vmService,
        UserService userService, IMongoCollection<LabEntity> labsEntityCollection, ProxmoxService proxmoxService)
    {
        _labsCollection = labsCollection;
        _vmsCollection = vmsCollection;
        _vmService = vmService;
        _userService = userService;
        _labsEntityCollection = labsEntityCollection;
        this.proxmoxService = proxmoxService;
    }

    public async Task<string> CreateLab(string Id)
    {
        var httpContext = new HttpContextAccessor().HttpContext;
        var userId = httpContext?.User.FindFirst(claim => claim.Type == "Id")?.Value;
        var count = await _vmsCollection.Find(_ => true).CountDocumentsAsync() + 1000;
	Console.WriteLine(count);
        await _vmService.CreateGroupVmAsync(count);

        LabEntity doc = new LabEntity()
        {
            Vms = new List<long>() { count + 1, count + 2, count + 3, count + 4 },
            UserId = userId
        };
        Vm ubuntu = new Vm()
        {
            VmId = (int)(count + 1),
            Name = "ubuntu",
        };
        Vm kali = new Vm()
        {
            VmId = (int)(count + 2),
            Name = "kali",
        };
        Vm xp = new Vm()
        {
            VmId = (int)(count + 3),
            Name = "xp",
        };
        Vm router = new Vm()
        {
            VmId = (int)(count + 4),
            Name = "router",
        };
        try
        {
            await _labsEntityCollection.InsertOneAsync(doc);
            var filter = Builders<Lab>.Filter.Eq("_id", ObjectId.Parse(Id));
            var results = (await _labsCollection.FindAsync(filter)).FirstOrDefault();
            var tmp = results.LabsEntitys.Append(doc.Id);
            var update = Builders<Lab>.Update.Set("LabsEntitys", tmp);
            await _labsCollection.UpdateOneAsync(filter, update);
            await _userService.UpdateAsync(userId, ObjectId.Parse(doc.Id));
            await _vmService.InsertMany(new List<Vm>{ubuntu, kali, xp, router});
            return doc.Id;
        }
        catch (Exception e)
        {
            throw new Exception();
        }
    }

    public async Task<List<Lab>> GetAllLabs()
    {
        return (await _labsCollection.FindAsync(_ => true)).ToList();
    }

    public async Task<List<object>> GetClusterStatus(string id)
    {
        var filter = Builders<LabEntity>.Filter.Eq("_id", ObjectId.Parse(id));
        var lab = (await _labsEntityCollection.FindAsync(filter)).FirstOrDefault();
        try
        {
            var kali = await proxmoxService.VmStatus(lab.Vms[0].ToString());
            var ubuntu = await proxmoxService.VmStatus(lab.Vms[1].ToString());
            var windows = await proxmoxService.VmStatus(lab.Vms[2].ToString());
            var router = await proxmoxService.VmStatus(lab.Vms[3].ToString());    
	return new List<object>()
            {
                ubuntu["data"], kali["data"],
                windows["data"], router["data"]
            };
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    public async Task<List<User>> GetLabUser(string id)
    {
        try
        {
            var filter = Builders<Lab>.Filter.Eq("_id", ObjectId.Parse(id));
            var lab = (await _labsCollection.FindAsync(filter)).FirstOrDefault();
            var labsEntityIds = lab.LabsEntitys;
            var g = (await _labsEntityCollection.FindAsync(x => labsEntityIds.Contains(x.Id))).ToList().Select(x => x.UserId);
            return  await _userService.GetLabUsersAsync(g);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
