using Microsoft.Build.Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Model.TestModels;

public class Test
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = "";
    
    [Required] 
    public string Name { get; set; } = null!;

    public string Description { get; set; }
    
    public List<string> Questions { get; set; }
}