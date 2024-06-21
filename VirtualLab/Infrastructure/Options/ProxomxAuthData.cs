using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Infrastructure;

public class ProxmoxAuthData
{
    private const string TOKEN = "PROXMOX_TOKEN";
    private const string IP_PROXMOX = "IP_PROXMOX";
    public string Token { get; private init; }
    public Ip Ip { get; private init; }

    public static ProxmoxAuthData FromEnv()
    {
        var token = Environment.GetEnvironmentVariable(TOKEN) ??
                    throw new ApplicationException("not token for proxmox");
        var ip = Environment.GetEnvironmentVariable(IP_PROXMOX) ?? throw new ApplicationException("not ip for proxmox");


        return new ProxmoxAuthData
        {
            Ip = new Ip { Value = ip },
            Token = token
        };
    }
}