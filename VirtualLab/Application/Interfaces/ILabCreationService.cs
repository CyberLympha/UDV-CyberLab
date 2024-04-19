using FluentResults;
using ProxmoxApi.Domen.Entities;

namespace VirtualLab.Application;

public interface ILabCreationService
{
    public Task<Result> Create(Lab lab);
    public Task<Result> Change(Lab lab);
    public Task<Result<Lab>> Get(Lab lab);
}