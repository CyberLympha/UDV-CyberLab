using System.Collections;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class QemuCollections 
{
    private readonly List<int> _list = [];

    public void CopyTo(Array array, int index)
    {
        throw new NotImplementedException();
    }

    public int Count => _list.Count;
    public bool IsSynchronized { get; }
    public object SyncRoot { get; }
    public int this[int i] => _list[i];

    public void AddRange(int from, int to)
    {
        for (var i = from; i <= to; i++) Add(i);
    }

    public void Add(int q)
    {
        _list.Add(q);
    }

}