namespace VirtualLab.Controllers.LabDistributionController.Dto;

public record LabCreateRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Manual { get; set; }
    public List<TemplateDataRequest> Template { get; set; }
}