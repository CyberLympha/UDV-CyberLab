using Amazon.Runtime.Internal.Transform;
using VirtualLab.Controllers.LabDistributionController.Dto;

namespace VirtualLab.Domain.Value_Objects.Proxmox;

// todo: сделать лучше: потецинально, мы будет не только bridge делать поэтому Type есть.
public class Net
{
    public string GetFull => string.Join(",", _parameters.Select(x => $"{x.Key}={x.Value}"));
    public string Bridge => _parameters["bridge"];

    // todo: пока что максимально простая реализация, потом допилить. builder с начальником
    public Net(string model, string bridge)
    {
        _parameters = new Dictionary<string, string>
        {
            { "model", model },
            { "bridge", bridge }
        };
        
        Type = "bridge";
    }

    private Net(Dictionary<string, string> parameters)
    {
        _parameters = parameters;
        Type = "bridge";
    }
    
    public string Type { get; }
    public string this[string index] => _parameters[index];
    public bool CanChange { get; set; }

    private readonly Dictionary<string, string> _parameters;

    public override string ToString()
    {
        return $"{Bridge}, {_parameters}";
    }


    public static Net From(NetRequest request)
    {
        return new Net(request.Parameters)
        {
            CanChange = request.CanChange
        };
    }
}