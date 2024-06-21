using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authorization;

public static class AuthOptions
{
    public const string ISSUER = "TestAuthServer";
    public const string AUDIENCE = "TestAuthClient";
    public const int EXPIRES_MINUTES = 10;
    private const string KEY = "mysupersecret_secretsecretsecretkey!123";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}