using MongoDB.Driver;
using WebApi.Model.TestModels;

namespace WebApi.Services;

public class TestsService
{
    private readonly IMongoCollection<Test> _testsCollection;

    public TestsService(IMongoCollection<Test> testsCollection)
    {
        _testsCollection = testsCollection;
    }

    public Task<List<Test>> Get
    {
        get { return _testsCollection.Find(_ => true).ToListAsync(); }
    }

    public Task<Test> GetById(string id)
    {
        return _testsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<string> Create(Test test)
    {
        await _testsCollection.InsertOneAsync(test);
        return test.Id;
    }
}