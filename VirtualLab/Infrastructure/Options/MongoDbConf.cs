namespace VirtualLab.Infrastructure.Options;

public record MongoDbConf(string UserName, string Password, int Port, string DataBase)
{
    private const string UsernameEnv = "MongoDbUserName";
    private const string PasswordEnv = "MongoDbPassword";
    private const string PortEnv = "MongoDbPort";
    private const string DataBaseEnv = "MongoDbDataBase";

    public MongoDbConf() : this(string.Empty, string.Empty, 4, string.Empty)
    {
    }

    public string UrlConnection()
    {
        return $"mongodb://root:example@localhost:{Port}";
    }

    public static MongoDbConf FromEnv()
    {
        var userName = GetEnvVar(UsernameEnv);
        var password = GetEnvVar(PasswordEnv);
        var port = GetEnvVar(PortEnv);
        var dataBase = GetEnvVar(DataBaseEnv);

        return new MongoDbConf(userName, password, int.Parse(port), dataBase);
    }


    //todo: вынести в extension.
    private static string GetEnvVar(string var)
    {
        return Environment.GetEnvironmentVariable(var) ??
               throw new ApplicationException($"Not Found var '{var}'. you must set this var");
    }
}