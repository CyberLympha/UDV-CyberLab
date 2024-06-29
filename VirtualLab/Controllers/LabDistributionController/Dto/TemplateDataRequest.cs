namespace VirtualLab.Controllers.LabDistributionController.Dto;

public record TemplateDataRequest(
    int Id, 
    string Password,
    string Name,
    string Node,
    List<NetRequest> Nets);