using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Infrastructure;

public class ProxmoxAuthData
{
    public string Token { get; private init; }
    public Ip Ip { get; private init; }
    private const string TOKEN = "PROXMOX_TOKEN";
    private const string IP_PROXMOX = "IP_PROXMOX";

    public static ProxmoxAuthData FromEnv()
    {
        var token = Environment.GetEnvironmentVariable(TOKEN) ?? "root@pam!devil=e094061e-e909-4415-9a57-f353a81a9eed";//throw new ApplicationException("not token for proxmox")
        var ip = Environment.GetEnvironmentVariable(IP_PROXMOX) ?? "10.40.229.10";//throw new ApplicationException("not ip for proxmox")
        
        
        return new ProxmoxAuthData()
        {
            Ip = new Ip() { Value = ip},
            Token = token
        };
    }
}