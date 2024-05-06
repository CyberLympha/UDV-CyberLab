namespace VirtualLab.Domain.Entities;

public class Credential : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid VmId { get; set; } // todo: потом нужно заменить на vm_id.
    public string Username { get; set; }
    public string Password { get; set; }
    public string Ip { get; set; }


    public static Credential From(string ip, string username, string password, Guid vmId)
        => new()
        {
            Id = Guid.NewGuid(),
            Password = password,
            Ip = ip,
            Username = username,
            VmId = vmId
        };
}