namespace VirtualLab.Domain.Entities.Mongo;

public class NetConfig
{
    public bool canChange { get; set; }
    public Dictionary<string, string> Parameters { get; init; }= new();
    
    
}