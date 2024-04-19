using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects;
using VirtualLab.Domain.Value_Objects.Proxmox;
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

    public async Task<Result<LabNodeConfig>> GenerateLabConfig(Guid labId)
    {
        var lab = await _labs.Get(labId);
        if (lab.IsFailed)
        {
            return Result.Fail(lab.Errors);
        }


        var nets = new NetCollection();
        nets.Add("virtio", "vmbr4");
        var labConfig = new LabNodeConfig()
        {
            Node = "1",
            CloneVmTemplates = new List<CloneVmTemplate>()
            {
                new()
                {
                    VmIdTemplate = 200,
                    NewId = 500,
                },
                new()
                {
                    VmIdTemplate = 201,
                    NewId = 501,
                }
            },
            Nets = nets
        };

        _log.Info($"configure created {labConfig}");
        return labConfig;
    }
}