using Microsoft.Build.Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models.LabWorks;

/// <summary>
/// Represents a laboratory work record.
/// </summary>
public record LabWork
{
    /// <summary>
    /// The id of the laboratory work
    /// </summary>
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    /// <summary>
    /// The id of the virtual machine that is template for the machine for the user to do the lab work
    /// </summary>
    [Required]
    public required string VmId { get; set; }
    
    /// <summary>
    /// The id of the lab work instruction
    /// </summary>
    [Required]
    public required string InstructionId { get; set; }
    
    /// <summary>
    /// Gets or sets the title of the laboratory work.
    /// </summary>
    [Required]
    public required string Title { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the short description of the laboratory work.
    /// </summary>
    [Required]
    public required string ShortDescription { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the detailed description of the laboratory work.
    /// </summary>
    [Required]
    public required string Description { get; set; } = null!;
}