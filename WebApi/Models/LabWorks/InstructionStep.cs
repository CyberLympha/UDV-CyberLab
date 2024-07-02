using Microsoft.Build.Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models.LabWorks;

/// <summary>
///     Represents a step in an instruction.
/// </summary>
public record InstructionStep
{
    /// <summary>
    ///     The unique identifier of the step.
    /// </summary>
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    ///     The instruction for this step.
    /// </summary>
    [Required]
    public required string Instruction { get; set; }

    /// <summary>
    ///     The hint for this step.
    /// </summary>
    [Required]
    public required string Hint { get; set; }

    /// <summary>
    ///     The list of possible answers for this step.
    /// </summary>
    [Required]
    public required List<string> Answers { get; set; }
}