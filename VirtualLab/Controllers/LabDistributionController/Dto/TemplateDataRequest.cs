using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Controllers.LabDistributionController.Dto;

public record TemplateDataRequest(
    int Id, 
    string Password,
    string Name,
    string Node,
    Dictionary<string,string> Nets);