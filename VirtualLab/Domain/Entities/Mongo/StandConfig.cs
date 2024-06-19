using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using VirtualLab.Controllers.LabDistributionController.Dto;

namespace VirtualLab.Domain.Entities.Mongo;

public class StandConfig : IEntity<ObjectId>
{
    [BsonId]
    public ObjectId Id { get; set; }
    
    public Guid LabId { get; set; }
    public List<TemplateVmConfig> TemplatesVmConfig { get; set; }

    public static StandConfig From(StandCreateRequest requestStandCreateRequest)
    {
        return new StandConfig()
        {
            TemplatesVmConfig = requestStandCreateRequest.TemplateConfigs,
            Id = ObjectId.GenerateNewId(),
            LabId = Guid.NewGuid()
        };
    }
}