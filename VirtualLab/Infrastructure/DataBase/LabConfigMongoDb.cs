using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VirtualLab.Infrastructure.Options;
using VirtualLab.Lib;

namespace VirtualLab.Infrastructure.DataBase;

public class LabConfigMongoDb : IMongoContext
{
    private IMongoDatabase _database;
    private IMongoClient _client;
    private IClientSessionHandle _session;
    private ConfMongoDb _confMongoDb;
    private readonly List<Func<Task>> _commands;
    
    public LabConfigMongoDb(
        IOptions<ConfMongoDb> confMongoDb
        )
    {
        _client = new MongoClient(confMongoDb.Value.UrlConnection());
        _confMongoDb = confMongoDb.Value;
        _database = _client.GetDatabase(confMongoDb.Value.Database);
    }
    
    

    public void Dispose()
    {
        _session?.Dispose();
        GC.SuppressFinalize(this);
    }

    public void AddCommand(Func<Task> command)
    {
        _commands.Add(command);
    }

    public async Task SaveChangeAsync()
    {
        using (_session = await _client.StartSessionAsync())
        {
            _session.StartTransaction();

            var commandTasks = _commands.Select(x => x());

            await Task.WhenAll(commandTasks);

            await _session.CommitTransactionAsync();
        }
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
    {
        return _database.GetCollection<TEntity>(name);
    }
}