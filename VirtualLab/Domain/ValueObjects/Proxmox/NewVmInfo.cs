using VirtualLab.Domain.ValueObjects.Proxmox.Config;

namespace VirtualLab.Domain.ValueObjects.Proxmox;

public record NewVmInfo
{
    public string? Ip { get; set; }
    public int ProxmoxVmId { get; set; }
    public string Node { get; set; }
    public string Password { get; set; }
    public string Username { get; set; }

    public static NewVmInfo From(CloneVmConfig cloneVmConfig)
    {
        return new NewVmInfo
        {
            ProxmoxVmId = cloneVmConfig.newQemu.Id,
            Password = cloneVmConfig.TemplateData.Password,
            Username = cloneVmConfig.TemplateData.Name,
            Node = cloneVmConfig.newQemu.Node
        };
    }
}