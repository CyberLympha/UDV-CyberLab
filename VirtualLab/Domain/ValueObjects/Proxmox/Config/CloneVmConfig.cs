namespace VirtualLab.Domain.ValueObjects.Proxmox.Config;

// todo: потенциаольно это можно обернуть методами, чтоб было все более интуитивно.
public record CloneVmConfig // в будущем добавить extentions, который будет доставать все Nets 
{
    public TemplateData TemplateData { get; init; }
    public int NewId { get; init; }
    // добавить сюда node для NewId и если, что созадть класс.
}

//todo: можно сделать словарь где ключ NewId. и все красиво обернуть.
//todo: в том плане, чтоб по NewId уже находить и Template и Nets