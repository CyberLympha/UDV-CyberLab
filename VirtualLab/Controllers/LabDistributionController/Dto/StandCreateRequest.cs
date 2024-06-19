using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Mongo;

namespace VirtualLab.Controllers.LabDistributionController.Dto;

public record StandCreateRequest(List<TemplateVmConfig> TemplateConfigs);