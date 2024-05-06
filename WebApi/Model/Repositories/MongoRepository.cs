using MongoDB.Driver;

namespace WebApi.Model.Repositories.MongoDbRepositories;

public class MongoRepository<T> : IRepository<T> where T : IIdentifiable
{
    private readonly IMongoCollection<T> _questionsCollection;

    public MongoRepository(IMongoCollection<T> questionsCollection)
    {
        _questionsCollection = questionsCollection;
    }

    public Task<T> Create(T itemToCreate)
    {
        _questionsCollection.InsertOneAsync(itemToCreate);
        return Task.FromResult(itemToCreate);
    }

    public Task<T> ReadById(string id) =>
        _questionsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public Task<IEnumerable<T>> ReadByRule(Func<T, bool> rule)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> ReadAll() =>
        Task.FromResult(_questionsCollection.Find(_ => true).ToEnumerable());

    public async Task<T> Update(T itemToUpdate)
    {
        var updatedItem = await _questionsCollection.FindOneAndReplaceAsync(x => x.Id == itemToUpdate.Id, itemToUpdate);
        return updatedItem;
    }

    public async void Delete(string id)
    {
        await _questionsCollection.DeleteOneAsync(x => x.Id == id);
    }
}