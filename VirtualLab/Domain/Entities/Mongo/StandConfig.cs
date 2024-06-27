using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VirtualLab.Domain.ValueObjects.Proxmox;

namespace VirtualLab.Domain.Entities.Mongo;

public class StandConfig : IEntity<ObjectId>
{
    public string Node => TemplateData[0].Node;
    
    
    public Guid LabId { get; set; }
    public List<TemplateData> TemplateData { get; set; }

    [BsonId] public ObjectId Id { get; set; }
 
    
    //представим, что у вcех template одна node 
    public static StandConfig From(CreateLabDto createLabDto)
    {
        return new StandConfig
        {
            Id = ObjectId.GenerateNewId(),
            LabId = createLabDto.Lab.Id,
            TemplateData = createLabDto.Templates
        };
    }
}