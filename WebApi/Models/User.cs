using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Models;

public enum UserRole
{
    Anon,
    User,
    Admin,
    SuperUser,
    Teacher,
}

public class User
{
    [BsonId]
    [Required]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;

    [Required] public string FirstName { get; set; } = null!;
    [Required] public string SecondName { get; set; } = null!;

    [Required] public string Email { get; set; } = null!;
    [JsonIgnore] public string Password { get; set; } = null!;

    [Required] public UserRole Role { get; set; } = UserRole.Anon;
    [Required] public string Labs { get; set; } = null!;
    [Required] public bool IsApproved { get; set; } = false;
}