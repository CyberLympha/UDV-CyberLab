using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Infrastructure.Extensions;

namespace VirtualLab.Application;

public class PveTemplateService : IPveTemplateService
{
    private readonly IProxmoxNetwork _network;

    public PveTemplateService(IProxmoxNetwork network)
    {
        _network = network;
    }

    public async Task<Result<TemplateData>> GetDataTemplate(int id, string node)
    {
        var getNets = await _network.GetAllNetworksBridgeByVm(id, node);
        if (!getNets.TryGetValue(out var nets, out var errors))
        {
            return Result.Fail(errors);
        }

        var templateData = new TemplateData()
        {
            Nets = nets,
            Id = id,
            Node = node
        };


        return templateData;
    }
}