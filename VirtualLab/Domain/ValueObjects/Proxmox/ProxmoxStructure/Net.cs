using Amazon.Runtime.Internal.Transform;
using VirtualLab.Controllers.LabDistributionController.Dto;
using VirtualLab.Domain.Entities.Mongo;

namespace VirtualLab.Domain.Value_Objects.Proxmox;

// todo: сделать лучше: потецинально, мы будет не только bridge делать поэтому Type есть.
public class Net
{
    public string GetFull => string.Join(",", Parameters.Select(x => $"{x.Key}={x.Value}"));
    public string Bridge => Parameters["bridge"];


    // todo: пока что максимально простая реализация, потом допилить. builder с начальником
    public Net(string model, string bridge)
    {
        Parameters.Add("model", model);
        Parameters.Add("bridge", bridge);
        Type = "bridge";
    }

    private Net(Dictionary<string, string> parameters)
    {
        Parameters = parameters;
        Type = "bridge";
    }

    public string Type { get; }
    public string this[string index] => Parameters[index];

    private readonly Dictionary<string, string> Parameters = new();

    public override string ToString()
    {
        return $"{Bridge}, {Parameters}";
    }


    public static Net From(NetConfig request)
    {
        return new Net(request.Parameters);
    }
}