using VirtualLab.Domain.Entities.Mongo;
using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Controllers.LabDistributionController.Dto;

public record StandCreateRequest(List<TemplateData> TemplateConfigs);