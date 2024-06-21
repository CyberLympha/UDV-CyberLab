using VirtualLab.Domain.Interfaces.MongoRepository;
using VirtualLab.Lib;

namespace VirtualLab.Infrastructure.UnitOfWork;

public class UnitOfWorkMongo : IUnitOfWork
{
    private readonly IMongoContext _context;

    public UnitOfWorkMongo(IMongoContext context,
        IConfigStandRepository configs)
    {
        _context = context;
        this.configs = configs;
    }

    public IConfigStandRepository configs { get; }

    public void Dispose()
    {
        _context.Dispose();
    }


    public async Task<bool> Commit()
    {
        var changeAmount = await _context.SaveChangeAsync();

        return changeAmount > 0;
    }
}