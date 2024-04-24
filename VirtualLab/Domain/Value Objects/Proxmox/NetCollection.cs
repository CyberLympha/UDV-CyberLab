using System.Collections;

namespace VirtualLab.Domain.Value_Objects.Proxmox;

public class NetCollection : IEnumerable<Net>
{
    private readonly Dictionary<int, Net> _nets = new();
    public int Count => _nets.Count;
    public IReadOnlyDictionary<int, string> Value => _nets.ToDictionary(x => x.Key, x => x.Value.GetFull);

    public void Add(NetSettings netSettings)
    {
        var net = new Net(netSettings.Model, netSettings.Bridge);

        _nets.Add(Count, net);
    }


    public IEnumerator<Net> GetEnumerator()
    {
        for (var i = 0; i < _nets.Count; i++)
        {
            yield return _nets[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}