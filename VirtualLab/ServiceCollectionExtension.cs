using Corsinvest.ProxmoxVE.Api;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
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
        var client = new PveClient("10.40.229.10");
        //todo: в будущем реализовать класс, который будет брать из переменных окружения токен.
        client.ApiToken = "root@pam!devil=e094061e-e909-4415-9a57-f353a81a9eed";

        services.AddSingleton<IProxmoxNetwork, Proxmox>();
        services.AddSingleton<IProxmoxVm, Proxmox>();

        services.AddSingleton<IStandManager, StandManager>();
        services.AddScoped<ILabConfigure, LabConfigure>();
        services.AddSingleton(client);
    }
}