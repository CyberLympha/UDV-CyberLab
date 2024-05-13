namespace WebApi.Models;

public record Log(TimeSpan Time, List<string> Arguments);