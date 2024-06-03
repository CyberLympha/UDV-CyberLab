using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VirtualLab.Domain.Entities;

public class StandConfig : IEntity<Guid>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    public List<ConfigTemplate> ConfigTemplate { get; set; }
    
}