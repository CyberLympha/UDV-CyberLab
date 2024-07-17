using System.Collections;
using VirtualLab.Domain.Value_Objects.Proxmox;

namespace VirtualLab.Domain.ValueObjects.Proxmox.ProxmoxStructure;

public class NetCollection : IEnumerable<Net> // todo: встваить сюда интерфейса листта либо коллекция
{
    private Dictionary<int, Net> _nets = new();
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

        Add(net);
    }

    public void Add(Net net)
    {
        //todo очень круто бы сделать проверку, что такое net может существовать
        _nets.Add(Tail, net);
    }

    public void AddRange(List<Net> nets)
    {
        foreach (var net in nets) Add(net);
    }
}