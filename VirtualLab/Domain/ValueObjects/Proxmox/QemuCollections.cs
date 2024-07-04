using System.Collections;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public class QemuCollections 
{
    private readonly List<int> _list = [];

    public QemuCollections(List<int> list)
    {
        _list = list;
    }
    
    public int Count => _list.Count;
    public int this[int i] => _list[i];

   

    public void Add(int q)
    {
        _list.Add(q);
    }

}