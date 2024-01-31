using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using System.Text;

namespace ApiGateWay.Config
{
    public static class JWTExtensions
    {
        public const string ValidIssuer = "http://localhost:5148";
        public const string ValidAudience = "http://localhost:5119";
        public const string authenticationProviderKey = "JWTWebMedclMyAppAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr";

        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                     {
                         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                     }).AddJwtBearer(options =>
                     {
                         options.RequireHttpsMetadata = false;
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,
                             ValidateAudience = true,
                             ValidateLifetime = true,
                             ValidIssuer = ValidIssuer,
                             ValidAudience = ValidAudience,
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationProviderKey)),
                         };
                     });

            services.AddOcelot();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader());
            });
        }
    }
}
