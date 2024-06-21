using FluentResults;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.Entities.Mongo;
using VirtualLab.Domain.Interfaces.Repositories;
using VirtualLab.Domain.ValueObjects.Proxmox;
using VirtualLab.Infrastructure.Extensions;
using VirtualLab.Lib;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Application;

public class LabCreationService : ILabCreationService
{
    private readonly ILog _log;
    private readonly IUnitOfWork _ofWorkMongoDb;
    private readonly ILabRepository labs;

    public LabCreationService(
        ILabRepository labs,
        ILog log,
        IUnitOfWork ofWorkMongoDb)
    {
        this.labs = labs;
        log.ForContext("creating lab");
        _log = log;
        _ofWorkMongoDb = ofWorkMongoDb;
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

    public async Task<Result> Create(CreateLabDto createLab)
    {
        var standConfig = StandConfig.From(createLab);

        var addedLab = await labs.Insert(createLab.Lab);
        if (addedLab.IsFailedWithErrors(out var errors)) return Result.Fail(errors);

        standConfig.Node = "pve"; //todo: убрать и сделать нормально))
        await _ofWorkMongoDb.configs.Insert(standConfig);
        await _ofWorkMongoDb.Commit();

        _log.Info($"lab {createLab.Lab.Name} with id {createLab.Lab.Id} created");
        return addedLab;
    }
}