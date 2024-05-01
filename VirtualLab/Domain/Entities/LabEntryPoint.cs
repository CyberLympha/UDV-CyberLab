using ProxmoxApi.Domen;

namespace VirtualLab.Domain.Entities;

public class LabEntryPoint : IEntity<System.Guid>
{
    public System.Guid Id { get; set; }
    public System.Guid UserLabId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Ip { get; set; }




    public static LabEntryPoint From(string ip, string username, string password, System.Guid labConfigLabId)
        => new LabEntryPoint()
        {
            Id = System.Guid.NewGuid(),
            Password = password,
            Ip = ip,
            Username = username,
            
        };
}
