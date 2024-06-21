using FluentResults;
using VirtualLab.Domain.Entities;
using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Application.Interfaces;

public interface ILabCreationService
{
    public Task<Result> Change(Lab guid);
    public Task<Result<Lab>> Get(Lab guid);
    Task<Result> Create(CreateLabDto createLab);
}