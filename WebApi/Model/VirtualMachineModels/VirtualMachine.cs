using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Model.VirtualMachineModels;

public class Vm
{
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    [Required] public int Vmid { get; set; } = 0;
    [Required] public string Name { get; set; } = null!;
}