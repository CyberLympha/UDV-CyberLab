namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class QemuCollections
{
    private readonly List<int> _list = [];

    public void AddRange(int from, int to)
    {
        for (var i = from; i <= to ; i++)
        {
            _list.Add(i);
        }
    }
    
    public int this[int i] => _list[i];
}