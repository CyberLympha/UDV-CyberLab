using VirtualLab.Domain.Interfaces.MongoRepository;

namespace VirtualLab.Lib;

public interface IUnitOfWork : IDisposable
{
    public IConfigStandRepository configs { get; }
    public Task<bool> Commit(); // todo: сделать реализацию с result
}