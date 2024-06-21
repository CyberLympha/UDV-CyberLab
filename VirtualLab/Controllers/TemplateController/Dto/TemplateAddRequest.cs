namespace VirtualLab.Controllers.TemplateController.Dto;

public class TemplateAddRequest
{
    public int templatePveId { get; set; }
    public string userName { get; set; }
    public string Paasword { get; set; }
    public Guid LabId { get; set; }
}