namespace VirtualLab.Infrastructure.Options;

public record MongoDbConf(string UserName, string Password, int Port, string DataBase, string Ip)
{
    private const string UsernameEnv = "MongoDbUserName";
    private const string PasswordEnv = "MongoDbPassword";
    private const string PortEnv = "MongoDbPort";
    private const string DataBaseEnv = "MongoDbDataBase";
    private const string IpEnv = "MongoDbIp";
    public MongoDbConf() : this(string.Empty, string.Empty, 4, string.Empty, string.Empty)
    {
    }

    public string UrlConnection()
    {
        return $"mongodb://root:example@{Ip}:{Port}";
    }

    public static MongoDbConf FromEnv()
    {
        var userName = GetEnvVar(UsernameEnv);
        var password = GetEnvVar(PasswordEnv);
        var port = GetEnvVar(PortEnv);
        var dataBase = GetEnvVar(DataBaseEnv);
        var ip = Environment.GetEnvironmentVariable(IpEnv) ?? "localhost";
        return new MongoDbConf(userName, password, int.Parse(port), dataBase, ip);
    }


    //todo: вынести в extension.
    private static string GetEnvVar(string var)
    {
        return Environment.GetEnvironmentVariable(var) ??
               throw new ApplicationException($"Not Found var '{var}'. you must set this var");
    }
}