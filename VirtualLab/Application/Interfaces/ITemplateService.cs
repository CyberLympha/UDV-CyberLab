using System.Net.Sockets;
using FluentResults;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface ITemplateService
{
    public Task<Result> Add(TemplateVm templateVm);
}