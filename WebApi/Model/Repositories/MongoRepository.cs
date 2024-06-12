using MongoDB.Driver;
using WebApi.Helpers;

namespace WebApi.Model.Repositories;

public class MongoRepository<T> : IRepository<T> where T : IIdentifiable
{
    private readonly IMongoCollection<T> _collection;

    public MongoRepository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public Task<T> Create(T itemToCreate)
    {
        _collection.InsertOneAsync(itemToCreate);
        return Task.FromResult(itemToCreate);
    }

    public Task<T> ReadById(string id) =>
        _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<IEnumerable<T>> ReadByRule(Func<T, bool> rule)
    {
        return _collection
            .Find(_ => true)
            .ToEnumerable()
            .Where(v => rule(v));
    }

    public Task<IEnumerable<T>> ReadAll() =>
        Task.FromResult(_collection.Find(_ => true).ToEnumerable());

    public async Task<T> Update(T itemToUpdate)
    {
        var updatedItem = await _collection.FindOneAndReplaceAsync(x => x.Id == itemToUpdate.Id, itemToUpdate);
        return updatedItem;
    }

    public async Task<ApiOperationResult> Delete(string id)
    {
        var deleteResult = await _collection.DeleteOneAsync(x => x.Id == id);
        return deleteResult.DeletedCount == 0
            ? Error.NotFound("Не удалось удалить вопрос с данным id")
            : ApiOperationResult.Success();
    }
}