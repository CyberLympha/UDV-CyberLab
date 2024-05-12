using Microsoft.Build.Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

/// <summary>
/// Represents a set of instructions for a laboratory work.
/// </summary>
public record LabWorkInstruction
{
    /// <summary>
    /// The unique identifier of the instruction.
    /// </summary>
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    /// <summary>
    /// The dictionary containing steps of the instruction.
    /// Key: step number, Value: step ID.
    /// </summary>
    [Required]
    public required Dictionary<string, string> Steps { get; set; }
}