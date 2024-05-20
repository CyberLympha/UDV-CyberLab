using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public class Vm
{
    /// <summary>
    /// Id of the vm record in db
    /// </summary>
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    
    /// <summary>
    /// Id of the vm in proxmox
    /// </summary>
    [Required] public int VmId { get; set; }
    
    /// <summary>
    /// Name of the vm in proxmox
    /// </summary>
    [Required] public string Name { get; set; } = null!;
    
    /// <summary>
    /// Id of the lab work record
    /// </summary>
    [Required]
    public string LabWorkId { get; set; }
}