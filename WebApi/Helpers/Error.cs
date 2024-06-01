using System.Net;
using Newtonsoft.Json;

namespace WebApi.Helpers;

[JsonObject]
public sealed class Error
{
    public static Error BadRequest(string message) => new(HttpStatusCode.BadRequest, message);
    public static Error NotFound(string message) => new(HttpStatusCode.NotFound, message);

    [JsonProperty("statusCode")] public HttpStatusCode StatusCode { get; }

    [JsonProperty("message")] public string Message { get; }

    [JsonConstructor]
    public Error(HttpStatusCode statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }

    private bool Equals(Error other) => StatusCode == other.StatusCode &&
                                        string.Equals(Message, other.Message,
                                            StringComparison.InvariantCultureIgnoreCase);

    public override int GetHashCode() => Message.GetHashCode() ^ (int)StatusCode;
    
    public ErrorResponse ToErrorResponse() =>
        new()
        {
            Message = Message
        };
}