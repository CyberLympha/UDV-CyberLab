using VirtualLab.Domain.Entities.Mongo;

namespace VirtualLab.Infrastructure.Extensions;

public static class ConfigExtensions
{
    public static int CountForChanged(this List<NetConfig> config)
    {
        return config.Count(net => net.canChange);
    }
}