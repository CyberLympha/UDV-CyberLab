namespace VirtualLab.Infrastructure.ApiResult;

public class WithDataBase
{
    public string NotInsert(string name, string id, string errors)
    {
        return $"Entity {name} with id {id} not insert because {errors}";
    }

    public string NotFound(string name, string id)
    {
        return $"Entity {name} with id {id} not found";
    }
}