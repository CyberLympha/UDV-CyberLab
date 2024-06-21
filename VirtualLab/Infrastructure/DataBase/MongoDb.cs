using MongoDB.Driver;
using VirtualLab.Infrastructure.Options;
using VirtualLab.Lib;
using Vostok.Logging.Abstractions;

namespace VirtualLab.Infrastructure.DataBase;

public class MongoDb : IMongoContext
{
    private readonly List<Func<Task>> _commands;
    private readonly IMongoClient _client;
    private readonly IMongoDatabase _database;
    private ILog _log;
    private MongoDbConf _mongoDbConf;
    private IClientSessionHandle _session;

    public MongoDb(MongoDbConf confMongoDb, ILog log)
    {
        _log = log;

        log.Debug($"{confMongoDb}");
        _client = new MongoClient(confMongoDb.UrlConnection());
        _mongoDbConf = confMongoDb;
        _database = _client.GetDatabase(confMongoDb.DataBase);
        _commands = new List<Func<Task>>();
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

    public async Task<int> SaveChangeAsync()
    {
        if (_commands.Count == 1)
        {
            await _commands[0].Invoke();

            return 1;
        }

        //todo: чтоб работало нужен replic-set
        using (_session = await _client.StartSessionAsync())
        {
            _session.StartTransaction();

            var commandTasks = _commands.Select(x => x());

            await Task.WhenAll(commandTasks);

            await _session.CommitTransactionAsync();
        }

        return
            _commands.Count; // по сути каждый новая транзация - это уже новый экзпляр класса(scoped), поэтому мы не очищаем commands. 
    }

    public IMongoCollection<TEntity> GetCollection<TEntity>(string name)
    {
        return _database.GetCollection<TEntity>(name);
    }
}