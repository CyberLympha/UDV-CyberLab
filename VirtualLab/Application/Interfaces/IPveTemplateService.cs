using FluentResults;
using VirtualLab.Domain.ValueObjects.Proxmox;
using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;

namespace VirtualLab.Application.Interfaces;

public interface IPveTemplateService
{
    public Task<Result<TemplateData>> GetDataTemplate(int id, string node);
}