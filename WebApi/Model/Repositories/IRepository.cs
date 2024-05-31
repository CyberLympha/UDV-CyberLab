using WebApi.Helpers;

namespace WebApi.Model.Repositories;

public interface IRepository<T> where T: IIdentifiable
{
    Task<T> Create(T itemToCreate);

    Task<T> ReadById(string id);

    Task<IEnumerable<T>> ReadByRule(Func<T, bool> rule);
    
    Task<IEnumerable<T>> ReadAll();

    Task<T> Update(T itemToUpdate);

    Task<ApiOperationResult> Delete(string id);
}