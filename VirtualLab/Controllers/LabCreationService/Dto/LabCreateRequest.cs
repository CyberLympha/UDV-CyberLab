namespace VirtualLab.Controllers.LabCreationService.Dto;

public class LabCreateRequest
{
    public string Name { get; set; }
    public string Goal { get; set; }
    public string Manual { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public long VmIdTemplate { get; set; }
    public long Node { get; set; }
}