using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.Value_Objects.Proxmox;
using VirtualLab.Domain.ValueObjects.Proxmox.Config;
using VirtualLab.Domain.ValueObjects.Proxmox.Requests;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Application;

public class LabConfigure : ILabConfigure
{
    private readonly ILabRepository _labs;
    private readonly ILog _log;

    public LabConfigure(ILabRepository labs, ILog log)
    {
        _labs = labs;
        log.ForContext("LabConfigure");
        _log = log;
    }
    
    //todo: псс, mongoDb норм идея, для хранием конфигов лаб)) по идей, могут быть доп данные.
    public async Task<Result<LabConfig>> GetConfigByLab(Guid labId)
    {
        var getTemplateConfig = await GetTemplateConfig(labId);
        if (getTemplateConfig.IsFailed) return Result.Fail(getTemplateConfig.Errors);

        var getConfigCurLab = await GenerateLabConfig(getTemplateConfig.Value);
        if (getConfigCurLab.IsFailed) return Result.Fail(getConfigCurLab.Errors);

        return getConfigCurLab;
    }

    private async Task<Result<LabConfig>> GetTemplateConfig(Guid labId)
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
        var labConfig = new LabConfig()
        {
            Node = "pve",
            CloneVmConfig = new List<CloneVmConfig>()
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
                    Nets = nets
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
                    Nets = nets
                }
            }
        };

        _log.Info($"configure created {labConfig}");
        return labConfig;
    }

    private async Task<Result<LabConfig>> GenerateLabConfig(LabConfig labConfig)
    {
        return labConfig;
    }
}