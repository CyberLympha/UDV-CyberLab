namespace VirtualLab.Infrastructure.Options;

public class ConfMongoDb
{
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }

    public string UrlConnection() => $"mongodb://root:example@localhost:{Port}";
}