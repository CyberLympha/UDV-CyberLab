using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.Value_Objects.Proxmox.Requests;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Application;

public class LabConfigureGenerate : ILabConfigureGenerate
{
    private readonly ILabRepository _labs;
    private readonly ILog _log;

    public LabConfigureGenerate(ILabRepository labs, ILog log)
    {
        _labs = labs;
        log.ForContext("LabConfigure");
        _log = log;
    }

    public async Task<Result<LabCreateRequest>> GenerateLabConfig(Guid labId)
    {
        var lab = await _labs.Get(labId);
        if (lab.IsFailed)
        {
            return Result.Fail(lab.Errors);
        }
        
        var nets = new NetCollection();
        nets.Add(new NetSettings
        {
            Bridge = "vmbr10",
            Model = "virtio"
        });
        var labConfig = new LabCreateRequest()
        {
            Node = "pve",
            ClonesRequest = new List<CloneRequest>()
            {
                new()
                {
                    Template = new Template()
                    {
                        WithVmbr0 = true,
                        Id = 104,
                        Name = "test",
                        Password = "test"
                    },
                    NewId = 500,
                },
                new()
                {
                    Template = new Template()
                    {
                        WithVmbr0 = false,
                        Id = 105,
                        Name = "test",
                        Password = "test"
                    },
                    NewId = 501,
                }
            },
            Nets = nets
        };

        _log.Info($"configure created {labConfig}");
        return labConfig;
    }
}