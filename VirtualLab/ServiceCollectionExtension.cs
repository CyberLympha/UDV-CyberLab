using Corsinvest.ProxmoxVE.Api;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;
using Vostok.Logging.Microsoft;
using LogLevel = Vostok.Logging.Abstractions.LogLevel;

namespace ProxmoxApi;

public static class ServiceCollectionExtension
{
    public static void AddLogVostokWithConfig(this WebApplicationBuilder builder)
    {
        var log = new ConsoleLog(new ConsoleLogSettings()
        {
            ColorsEnabled = true,
            ColorMapping = new Dictionary<LogLevel, ConsoleColor>() { { LogLevel.Info, ConsoleColor.DarkMagenta } }
        });


        builder.Logging.AddVostok(log);
        builder.Services.AddSingleton<ILog>(log);
    }

    public static void AddPveClient(this IServiceCollection services)
    {
        var client = new PveClient("10.40.229.61");
        //todo: в будущем реализовать класс, который будет брать из переменных окружения токен.
        client.ApiToken = "root@pam!devil=43cd358d-e00e-401b-9178-1a5c69e9685f";

        services.AddSingleton<INetworkService, Proxmox>();
        services.AddSingleton<IVmService, Proxmox>();

        services.AddSingleton<ILabVirtualMachineManager, LabVirtualMachineManager>();
        services.AddScoped<ILabConfigure, LabConfigure>();
        services.AddSingleton(client);
    }
}