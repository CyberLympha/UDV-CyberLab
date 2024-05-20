using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using WebApi.Model.Repositories;

namespace WebApi.Model.QuestionModels;

public class Question : IIdentifiable
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = "";

    public string Text { get; set; }

    public string Description { get; set; }

    public QuestionType QuestionType { get; set; }

    public string CorrectAnswer { get; set; }
    
    public string QuestionData { get; set; }
}