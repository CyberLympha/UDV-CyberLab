using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Authorization
{
    public static class ApplicationServiceExtensions
    {
        public static void AddConfigureAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidIssuer = AuthOptions.ISSUER,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Teacher", new AuthorizationPolicyBuilder()
                    .RequireRole("Teacher")
                    .RequireAuthenticatedUser()
                    .Build());
                options.AddPolicy("Student", new AuthorizationPolicyBuilder()
                    .RequireRole("Student")
                    .RequireAuthenticatedUser()
                    .Build());
                options.AddPolicy("Authenticated", new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());
            }
);
        }
    }
}
