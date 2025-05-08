using Delta;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Web.Constants;
using Web.Contexts;
using Web.Services;

namespace Web.Endpoints;
public static class DictionarySettingsEndpoint
{
    private const string PermissionForTest = "CanEditAnimal";
    private const string CacheKey = "availableProperties";
    private const int CacheDuration = 1;

    public static IEndpointRouteBuilder AddDictionarySettingsEndpoints(this IEndpointRouteBuilder app)
    {
        //{id:guid}
        app.MapGet("api/dic/availableProperties", async (
            [FromServices] WebDbContext context,
            [FromServices] IMemoryCache cache,
            // [FromRoute] Guid id,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken) =>
        {
             var httpContext = httpContextAccessor.HttpContext;
            
             if (httpContext == null)
                 return Results.Problem("HttpContext is not available.");
            
             // IHttpContextAccessor httpContextAccessor jwt get permissions and check if available
             var userPermissions = httpContext.User.Claims
                 .Where(c => c.Type == AuthConstants.Permissions)
                 .Select(c => c.Value)
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
                     //TODO если для теста комментарий x.Id == id &&
                     x.StartDate <= now &&
                     x.EndDate >= now &&
                     x.Permissions.Any(p => userPermissions.Contains(p.Name)))
                 .Select(x => new 
                 {
                     x.AvailableDictionaries,
                     x.Permissions
                 }).FirstOrDefaultAsync(cancellationToken);

             if (data == null)
                 return Results.Forbid();
            
             //TODO something to get properties from availableProperties
             
             var availableProperties = await context.DictionaryEntities
                 .Where(d => data.AvailableDictionaries.Contains(d.Id))
                 .Select(d => new
                 {
                     d.Id,
                     d.Title
                 }).ToListAsync(cancellationToken);
            
             var result = new
             {
                 AvailableDictionaries = availableProperties,
                 Permissions = data.Permissions.Select(p => p.Name).ToList()
             };
            
             //TODO cache for hour
             cache.Set(CacheKey, data, TimeSpan.FromHours(CacheDuration));

             return Results.Ok(new { data, result });

             //TODO super check if in DB changed (Delta nuget package)
        }).UseDelta<WebDbContext>();

        return app;
    }
}