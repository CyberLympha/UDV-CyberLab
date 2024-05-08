using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authorization
{
    public static class AuthOptions
    {
        public const string ISSUER = "TestAuthServer";
        public const string AUDIENCE = "TestAuthClient";
        public const int EXPIRES_MINUTES = 10;
        const string KEY = "mysupersecret_secretsecretsecretkey!123";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
