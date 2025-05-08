using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Constants;
using Web.Contexts;
using Web.Endpoints.Requests;
using Web.Services;

namespace Web.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder AddUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("login/string", async (
            [FromBody] TestUserLoginRequest request,
            [FromServices] WebDbContext dbContext,
            [FromServices] IJwtService jwtService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var user = await dbContext.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Results.Unauthorized();

            var token = jwtService.GenerateToken(user);

            httpContextAccessor.HttpContext!.Response.Cookies.Append(AuthConstants.AccessToken, token);
            
            return Results.Ok(new { token });
        });

        app.MapPost("login/admin", async (
            [FromBody] TestAdminLoginRequest request,
            [FromServices] WebDbContext dbContext,
            [FromServices] IJwtService jwtService,
            [FromServices] IHttpContextAccessor httpContextAccessor) =>
        {
            var user = await dbContext.Users
                .Include(u => u.Permissions)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null)
                return Results.Unauthorized();

            var token = jwtService.GenerateToken(user);

            httpContextAccessor.HttpContext!.Response.Cookies.Append(AuthConstants.AccessToken, token);
            
            return Results.Ok(new { token });
        });

        return app;
    }
}