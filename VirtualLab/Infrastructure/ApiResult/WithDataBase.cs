namespace VirtualLab.Infrastructure.ApiResult;

public class WithDataBase
{
    public string NotInsert(string name, string id, string errors) =>
        $"Entity {name} with id {id} not insert because {errors}";
}