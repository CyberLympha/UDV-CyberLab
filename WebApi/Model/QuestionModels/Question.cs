using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApi.Model.QuestionModels;

public class Question
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = "";

    public string Text { get; set; }

    public string Description { get; set; }

    public QuestionType QuestionType { get; set; }

    public string CorrectAnswer { get; set; }
    
    public string QuestionData { get; set; }
}