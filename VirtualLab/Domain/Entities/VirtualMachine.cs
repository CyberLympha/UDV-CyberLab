namespace VirtualLab.Domain.Entities;

public class VirtualMachine : IEntity<Guid>
{
    public int ProxmoxVmId { get; set; }
    public Guid UserLabId { get; set; }
    public string Node { get; set; }
    public Guid Id { get; set; }

    public static VirtualMachine From(string node, int proxmoxVmId, Guid userLabId)
    {
        return new VirtualMachine
        {
            Id = Guid.NewGuid(),
            ProxmoxVmId = proxmoxVmId,
            Node = node,
            UserLabId = userLabId
        };
    }
}