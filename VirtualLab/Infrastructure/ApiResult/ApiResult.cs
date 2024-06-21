namespace VirtualLab.Infrastructure.ApiResult;

public class ApiResultError
{
    public static WithProxmox WithProxmox => new();
    public static WithDataBase WithDataBase => new();

    public static string NotFound(string nameEntity)
    {
        return $"Not Found {nameEntity}";
    }
}