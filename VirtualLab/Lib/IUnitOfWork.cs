namespace VirtualLab.Lib;

public interface IUnitOfWork : IDisposable
{
    public Task<bool> Commit();
}