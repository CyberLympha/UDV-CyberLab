using Microsoft.AspNetCore.Components.RenderTree;
using VirtualLab.Controllers.LabDistributionController.Dto;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Mongo;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class CreateLabDto
{
    public Lab Lab { get; init; }
    public List<TemplateConfig> Templates { get; init; }

    public static CreateLabDto From(LabCreateRequest request)
    {
        return new CreateLabDto
        {
            Lab = Lab.From(request),
            Templates = request.Template
        };
    }
}