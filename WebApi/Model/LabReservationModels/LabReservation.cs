using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebApi.Model.AuthModels;
using WebApi.Model.LabModels;

namespace WebApi.Model.LabReservationModels;
public class LabReservation
{
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [Required]
    [BsonDateTimeOptions(
    Kind = DateTimeKind.Local,
    Representation = BsonType.Document)]
    public DateTime TimeStart { get; set; }

    [Required]
    [BsonDateTimeOptions(
    Kind = DateTimeKind.Local,
    Representation = BsonType.Document)]
    public DateTime TimeEnd { get; set; }

    [Required] public string Theme { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public User Reservor { get; set; } = null!;
    [Required] public Lab Lab { get; set; } = null!;

    public bool Intersects(LabReservation other)
    {
        if (other.TimeStart < TimeEnd && other.TimeStart >= TimeStart) return true;
        if (TimeStart < other.TimeEnd && TimeStart >= other.TimeStart) return true;
        if (other.TimeEnd <= TimeEnd && other.TimeEnd > TimeStart) return true;
        if (TimeEnd <= other.TimeEnd && TimeEnd > other.TimeStart) return true;
        return false;
    }
}
