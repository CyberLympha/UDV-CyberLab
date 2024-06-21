namespace VirtualLab.Domain.Entities;

public class Credential : IEntity<Guid>
{
    public Guid VmId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Ip { get; set; }
    public Guid Id { get; set; }


    public static Credential From(string ip, string username, string password, Guid vmId)
    {
        return new Credential
        {
            Id = Guid.NewGuid(),
            Password = password,
            Ip = ip,
            Username = username,
            VmId = vmId
        };
    }
}