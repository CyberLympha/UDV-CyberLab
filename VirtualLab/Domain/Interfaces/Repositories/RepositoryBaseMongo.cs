using FluentResults;
using MongoDB.Driver;
using VirtualLab.Lib;

namespace VirtualLab.Domain.Interfaces.Repositories;

public abstract class RepositoryBaseMongo<TEntity, TId> : IRepositoryBase<TEntity, TId>
{
    protected readonly IMongoContext DbContext;
    protected readonly IMongoCollection<TEntity> DbSet;

    protected RepositoryBaseMongo(IMongoContext dbContext)
    {
        DbContext = dbContext;

        DbSet = dbContext.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    public async Task<Result<TEntity>> Get(TId id)
    {
        var data = await DbSet.FindAsync(Builders<TEntity>.Filter.Eq("_id",
            id)); // todo: как варик эти фильтры можно будет убрать за какой-нибудь класс для удобства.

        return data.SingleOrDefault();
    }

    public async Task<Result> Insert(TEntity entity)
    {
        DbContext.AddCommand(() => DbSet.InsertOneAsync(entity));

        return Result.Ok();
    }

    public async Task<Result<IReadOnlyCollection<TEntity>>> GetAll()
    {
        var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);

        return all.ToList();
    }
}