using MongoDB.Driver;

namespace VirtualLab.Lib;

public interface IMongoContext : IDisposable
{
    public void AddCommand(Func<Task> command);
    
    public Task SaveChangeAsync();

    public IMongoCollection<TEntity> GetCollection<TEntity>(string name);
}