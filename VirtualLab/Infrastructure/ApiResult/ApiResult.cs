using FluentResults;

namespace VirtualLab.Infrastructure.ApiResult;

public class ApiResultError
{
    public static ProxmoxErrors WithProxmox => new();
    public static WithDataBase WithDataBase => new();
 
    public static string NotFound(string nameEntity)
    {
        return $"Not Found {nameEntity}";
    }
}