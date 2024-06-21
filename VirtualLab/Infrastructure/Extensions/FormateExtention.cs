namespace VirtualLab.Infrastructure.Extensions;

public static class FormateExtention
{
    public static string ToUpFirst(this string line)
    {
        return char.ToUpper(line[0]) + line[1..].ToLower();
    }
}