namespace WebApi.Model.Exceptions;

public class AttemptException : Exception
{
    public AttemptException(string message)
    {
        Message = message;
    }
    public string Message { get; set; }
}