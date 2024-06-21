using VirtualLab.Controllers.LabDistributionController.Dto;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Mongo;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class CreateLabDto
{
    public Lab Lab { get; private init; }
    public List<TemplateVmConfig> Templates { get; private init; }

    public static CreateLabDto From(LabCreateRequest request)
    {
        return new CreateLabDto
        {
            Lab = Lab.From(request),
            Templates = request.StandCreateRequest.TemplateConfigs
        };
    }
}