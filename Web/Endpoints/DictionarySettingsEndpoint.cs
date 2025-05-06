using System.Diagnostics.CodeAnalysis;
using Delta;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Web.Contexts;
using Web.Domain;

namespace Web.Endpoints;

public static class DictionarySettingsEndpoint
{
    private const string EndpointGroupName = "DictionarySettings";
    private const string CacheKey = "availableProperties";
    private const int CacheDuration = 1;

    public static void AddDictionarySettingsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/dic/availableProperties/{id:guid}", async (
            [FromServices] WebDbContext context,
            [FromServices] IMemoryCache cache,
            [FromRoute] Guid id,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken) =>
        {
            var httpContext = httpContextAccessor.HttpContext;
            
            if (httpContext == null)
                return Results.Problem("HttpContext is not available.");
            
            //IHttpContextAccessor httpContextAccessor jwt get permissions and check if available
            var userPermissions = httpContext.User.Claims
                .Where(c => c.Type == "permissions")
                .Select(c => c.Value)
                .Distinct()
                .ToList();
            
            if (userPermissions.Count == 0)
                return Results.Unauthorized();

            if (cache.TryGetValue(CacheKey, out var cachedData))
            {
                return Results.Ok(cachedData);
            }

            var now = DateTime.UtcNow;

            //TODO check permissions is available
            var data = await context.DictionarySettingsEntities
                .Where(x =>
                    x.StartDate <= now &&
                    x.EndDate >= now &&
                    x.Permissions.Any(p => p.Name == "CanEditAnimal"))
                .Select(x => new
                {
                    x.AvailableDictionaries,
                    x.Permissions
                })
                .FirstOrDefaultAsync(cancellationToken);

            //TODO reflection or something else to get properties from aviableProperties
            if (data == null)
                return Results.Forbid();

            //TODO cache for hour
            cache.Set(CacheKey, data, TimeSpan.FromHours(CacheDuration));

            return Results.Ok(data);

            //TODO super check if in DB changed (Delta nuget package)
        }); /*.UseDelta<WebDbContext>();*/ //local timestamp exception
    }
}