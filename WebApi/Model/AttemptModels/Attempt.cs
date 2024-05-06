using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebApi.Model.Repositories;


namespace WebApi.Model.AttemptModels;

public class Attempt : IIdentifiable
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string TestId { get; set; }
    public string ExamineeId { get; set; }
    public AttemptStatus Status { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Dictionary<string, string> Results { get; set; }
}