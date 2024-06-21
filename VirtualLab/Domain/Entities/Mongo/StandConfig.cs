using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Domain.Entities.Mongo;

public class StandConfig : IEntity<ObjectId>
{
    public Guid LabId { get; set; }
    public List<TemplateVmConfig> TemplatesVmConfig { get; set; }
    public string Node { get; set; }

    [BsonId] public ObjectId Id { get; set; }

    public static StandConfig From(CreateLabDto createLabDto)
    {
        return new StandConfig
        {
            Id = ObjectId.GenerateNewId(),
            LabId = createLabDto.Lab.Id,
            TemplatesVmConfig = createLabDto.Templates
        };
    }
}