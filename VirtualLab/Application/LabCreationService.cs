using FluentResults;
using ProxmoxApi.Domen.Entities;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Interfaces.Repositories;
using Vostok.Logging.Abstractions;
using Guid = VirtualLab.Domain.Entities.Guid;

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

    public Task<Result> Change(Guid guid)
    {
        // todo: разные сложные проверки, на то, что лабу можно создать т.д

        throw new NotImplementedException();
    }

    public async Task<Result<Guid>> Get(Guid guid)
    {
        // todo: разные сложные проверки, на то, что лабу можно создать т.д
        // проверки на idVm.  есть ли такая node.

        var result = await labs.Get(guid.Id);
        return result;
    }

    public async Task<Result> Create(Guid guid)
    {
        // todo: разные сложные проверки, на то, что лабу можно создать т.д
        

        var x = await labs.Insert(guid);
        _log.Info($"lab {guid.Name} with id {guid.Id} created");
        
        return x;
    }
}