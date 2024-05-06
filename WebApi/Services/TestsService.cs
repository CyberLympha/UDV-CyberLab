using WebApi.Model.Repositories;
using WebApi.Model.TestModels;

namespace WebApi.Services;

public class TestsService
{
    private readonly IRepository<Test> _repository;

    public TestsService(IRepository<Test> repository)
    {
        _repository = repository;
    }

    public Task<IEnumerable<Test>> Get => _repository.ReadAll();

    public Task<Test> GetById(string id) => _repository.ReadById(id);

    public async Task<string> Create(Test test)
    {
        var newTest = await _repository.Create(test);
        return newTest.Id;
    }
}