using Microsoft.Build.Framework;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models.LabWorks;

/// <summary>
///     Represents the result of a lab work for a user.
/// </summary>
public record UserLabResult
{
    /// <summary>
    ///     The unique identifier of the user lab result.
    /// </summary>
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    /// <summary>
    ///     The identifier of the user associated with this lab result.
    /// </summary>
    [Required]
    public required string UserId { get; set; }

    /// <summary>
    ///     The identifier of the lab work associated with this result.
    /// </summary>
    [Required]
    public required string LabWorkId { get; set; }

    /// <summary>
    ///     Indicates whether the lab work is finished for this user.
    /// </summary>
    [Required]
    public required bool IsFinished { get; set; }

    /// <summary>
    ///     The current step reached by the user in the lab work.
    /// </summary>
    [Required]
    public required int CurrentStep { get; set; }
}