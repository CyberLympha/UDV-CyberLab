namespace VirtualLab.Infrastructure.ApiResult;

public class ApiResultError
{
    public static string NotFound(string nameEntity) => $"Not Found {nameEntity}";
    
    public static WithProxmox WithProxmox => new WithProxmox();
    public static WithDataBase WithDataBase => new WithDataBase();

}


