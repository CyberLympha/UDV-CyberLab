using System.Net.Sockets;
using FluentResults;
using VirtualLab.Controllers.TemplateController.Dto;
using VirtualLab.Domain.Entities;

namespace VirtualLab.Application.Interfaces;

public interface ITemplateService
{
    public Task<Result> Add(TemplateVm templateVm);

    public Task<Result<IReadOnlyCollection<TemplateVmInfo>>> GetAll();
}