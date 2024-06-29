using System.Text.Json.Serialization;

namespace VirtualLab.Controllers.LabDistributionController.Dto;

public class NetRequest
{
    public bool CanChange { get;  }
    [JsonRequired]
    public Dictionary<string,string> Parameters { get; init; }
}