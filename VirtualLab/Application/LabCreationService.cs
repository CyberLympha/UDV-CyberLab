using FluentResults;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Application;

public class LabCreationService : ILabCreationService
{
    private readonly ILabRepository labs;
    private readonly ILog _log;
    public LabCreationService(ILabRepository labs, ILog log)
    {
        this.labs = labs;
        log.ForContext("creating lab");
        _log = log;
    }

    public Task<Result> Change(Lab lab)
    {
        // todo: разные сложные проверки, на то, что лабу можно создать т.д

        throw new NotImplementedException();
    }

    public async Task<Result<Lab>> Get(Lab lab)
    {
        // todo: разные сложные проверки, на то, что лабу можно создать т.д
        // проверки на idVm.  есть ли такая node.

        var result = await labs.Get(lab.Id);
        return result;
    }

    public async Task<Result> Create(Lab lab)
    {
        // todo: разные сложные проверки, на то, что лабу можно создать т.д
        

        var x = await labs.Insert(lab);
        _log.Info($"lab {lab.Name} with id {lab.Id} created");
        
        return x;
    }
}