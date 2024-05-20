namespace VirtualLab.Domain.ValueObjects.Proxmox;

public enum ProxmoxVmStatus
{
    running,
    stopped // todo: кринж но так надо, proxmox с маленькой возваращет а время щя мало.
}