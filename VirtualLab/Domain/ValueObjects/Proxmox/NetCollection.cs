using System.Collections;

namespace VirtualLab.Domain.Value_Objects.Proxmox;

public class NetCollection : IEnumerable<Net>
{
    private readonly Dictionary<int, Net> _nets = new();
    private int Count => _nets.Count + 1; // никита блуа, это сложная логика))) потому что 0 это vmbr0
    public IReadOnlyDictionary<int, string> Value => _nets.ToDictionary(x => x.Key, x => x.Value.GetFull);

    public void Add(NetSettings netSettings)
    {
        var net = new Net(netSettings.Model, netSettings.Bridge);

        _nets.Add(Count, net);
    }


    public IEnumerator<Net> GetEnumerator() 
    {
        for (var i = 1; i < _nets.Count + 1; i++) // 0 это vmbr0 //todo: это очень неприятный костыль сейчас.
        {
            yield return _nets[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}