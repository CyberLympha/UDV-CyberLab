using Corsinvest.ProxmoxVE.Api;
using VirtualLab.Application;
using VirtualLab.Application.Interfaces;
using VirtualLab.Domain.Interfaces.Proxmox;
using VirtualLab.Infrastructure;
using VirtualLab.Infrastructure.Pve;
using Vostok.Logging.Abstractions;
using Vostok.Logging.Console;
using Vostok.Logging.Microsoft;
using LogLevel = Vostok.Logging.Abstractions.LogLevel;

namespace ProxmoxApi;

public static class ServiceCollectionExtension
{
    public static void AddLogVostokWithConfig(this WebApplicationBuilder builder)
    {
        var log = new ConsoleLog(new ConsoleLogSettings
        {
            ColorsEnabled = true,
            ColorMapping = new Dictionary<LogLevel, ConsoleColor> { { LogLevel.Info, ConsoleColor.DarkMagenta } }
        });


        builder.Logging.AddVostok(log);
        builder.Services.AddSingleton<ILog>(log);
    }

    public static void AddPveClient(this IServiceCollection services)
    {
        var authData = ProxmoxAuthData.FromEnv();
        var client = new PveClient(authData.Ip.Value)
        {
            ApiToken = authData.Token
        };

        services.AddSingleton<IProxmoxNetwork, Proxmox>();
        services.AddSingleton<IProxmoxVm, Proxmox>();
        services.AddSingleton<IProxmoxNode, Proxmox>();


        services.AddScoped<IProxmoxResourceManager, ProxmoxResourceManager>();

        services.AddSingleton<IStandManager, StandManager>();
        services.AddScoped<ILabConfigure, LabConfigure>();
        services.AddSingleton(client);
        services.AddSingleton(authData);
    }
}