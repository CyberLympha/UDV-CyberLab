namespace WebApi.Model.Exceptions;

public class IncorrectDataException : Exception
{
    public IncorrectDataException(string message)
    {
        Message = message;
    }
    public string Message { get; set; }
}