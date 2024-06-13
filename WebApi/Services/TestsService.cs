using WebApi.Helpers;
using WebApi.Model.Repositories;
using WebApi.Model.TestModels;

namespace WebApi.Services;

public class TestsService
{
    private readonly IRepository<Test> _repository;
    private readonly IdValidationHelper _idValidationHelper;

    public TestsService(IRepository<Test> repository, IdValidationHelper idValidationHelper)
    {
        _repository = repository;
        _idValidationHelper = idValidationHelper;
    }

    public Task<IEnumerable<Test>> Get => _repository.ReadAll();

    public async Task<ApiOperationResult<Test>> GetById(string id)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(id);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;
        
        return await _repository.ReadById(id).ConfigureAwait(false);
    }

    public async Task<ApiOperationResult<string>> Create(Test test)
    {
        var newTest = await _repository.Create(test).ConfigureAwait(false);
        return newTest.Id;
    }

    public async Task<ApiOperationResult> Delete(string id)
    {
        var idValidationResult = _idValidationHelper.EnsureValidId(id);
        if (!idValidationResult.IsSuccess)
            return idValidationResult.Error;

        return await _repository.Delete(id).ConfigureAwait(false);
    }
}