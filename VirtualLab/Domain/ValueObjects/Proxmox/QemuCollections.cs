namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class QemuCollections
{
    private readonly List<int> _list = [];

    public int Count => _list.Count;
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