using System.Text.Json.Serialization;

namespace VirtualLab.Controllers.LabDistributionController.Dto;

public class NetRequest
{
    public bool CanChange { get; init; }
    public Dictionary<string, string> Parameters { get; init; }
}