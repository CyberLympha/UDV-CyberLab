using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace WebApi.Models;

public class News
{
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public string Id { get; set; } = null!;
    
    [Required] public string Title { get; set; }
    [Required] public string Text { get; set; }
    [Required] public string CreatedAt { get; set; }
}