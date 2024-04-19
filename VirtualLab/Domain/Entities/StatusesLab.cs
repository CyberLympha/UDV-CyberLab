using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;

namespace ProxmoxApi.Domen.Entities;

public class StatusesLab : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}