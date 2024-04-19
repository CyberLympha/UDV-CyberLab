using System.Collections;

namespace VirtualLab.Domain.Value_Objects.Proxmox;

public class NetCollection : IEnumerable<KeyValuePair<int, string>>
{
    private int CountNet;
    private readonly List<Net> _nets = new();
    public IReadOnlyDictionary<int, string> Get => _nets.ToDictionary(n => n.Get.Key, n => n.Get.Value);

    public void Add(string model, string bridge)
    {
        var net = new Net(model, bridge, ++CountNet);

        _nets.Add(net);
    }


    public IEnumerator<KeyValuePair<int, string>> GetEnumerator()
    {
        foreach (var net in _nets)
        {
            yield return net.Get;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}