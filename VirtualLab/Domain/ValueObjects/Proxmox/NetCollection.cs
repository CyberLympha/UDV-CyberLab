using System.Collections;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class NetCollection : IEnumerable<Net> // todo: встваить сюда интерфейса листта
{
    private readonly Dictionary<int, Net> _nets = new();
    private int Tail => _nets.Count; // такое cебе название.

    public int Count => _nets.Count;
    public IReadOnlyDictionary<int, string> Value => _nets.ToDictionary(x => x.Key, x => x.Value.GetFull);

    public IEnumerator<Net> GetEnumerator()
    {
        for (var i = 0; i < _nets.Count; i++) yield return _nets[i];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(NetSettings netSettings)
    {
        var net = new Net(netSettings.Model, netSettings.Bridge);

        _nets.Add(Tail, net);
    }
}