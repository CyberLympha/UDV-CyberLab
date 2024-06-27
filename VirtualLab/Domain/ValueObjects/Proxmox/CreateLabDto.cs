using VirtualLab.Controllers.LabDistributionController.Dto;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class CreateLabDto
{
    public Lab Lab { get; init; }
    public List<TemplateData> Templates { get; init; }

    public static CreateLabDto From(LabCreateRequest request)
    {
        return new CreateLabDto
        {
            Lab = Lab.From(request),
            Templates = request.StandCreateRequest.TemplateConfigs
        };
    }
}