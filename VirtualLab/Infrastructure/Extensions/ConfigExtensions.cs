using VirtualLab.Domain.Entities.Mongo;

namespace VirtualLab.Infrastructure.Extensions;

public static class ConfigExtensions
{
    public static int CountNetChange(this List<TemplateConfig> templateConfigs)
    {
        var count = 0;

        var isCounted = new HashSet<string>();
        foreach (var templateConfig in templateConfigs)
        foreach (var net in templateConfig.Nets)
        {
            
            if (!net.canChange || isCounted.Contains(net.Parameters["bridge"])) continue;

            
            count++;
            isCounted.Add(net.Parameters["bridge"]);
        }

        return count;
    }
}
