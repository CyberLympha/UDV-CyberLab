using VirtualLab.Lib;

namespace VirtualLab.Infrastructure.UnitOfWork;

public class UnitOfWorkMongo : IUnitOfWork
{
    private readonly IMongoContext _context;

    public UnitOfWorkMongo(IMongoContext context)
    {
        _context = context;
    }

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