using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;
using Web.Constants;
using Web.Services;

namespace Web;

public static class ProgramExtension
{
    public static void AddJwtAuth(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IJwtService, JwtService>();
        
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
                };

                // Настроим обработку ошибок
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Cookies[AuthConstants.AccessToken];
                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    },
                    // OnAuthenticationFailed = context =>
                    // {
                    //     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //     context.Response.ContentType = "application/json";
                    //     return Task.CompletedTask;
                    // },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.ContentType = "application/json";
                        return Task.CompletedTask;
                    }
                };
            });
        builder.Services.AddAuthorization();
    }
}
