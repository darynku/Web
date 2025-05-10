using System.Text.Json;
using Delta;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Web.Constants;
using Web.Contexts;
using Web.Endpoints.Requests;

namespace Web.Endpoints;
public static class DictionarySettingsEndpoint
{
    private const string CacheKey = "availableProperties";
    private const int CacheDuration = 1;

    public static IEndpointRouteBuilder AddDictionarySettingsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/dic/availableProperties/{id:guid}", async (
            [FromServices] WebDbContext context,
            [FromServices] IDistributedCache cache,
            [FromRoute] Guid id,
            [FromServices] IHttpContextAccessor httpContextAccessor,
            CancellationToken cancellationToken) =>
        {
             var httpContext = httpContextAccessor.HttpContext;
            
             if (httpContext == null)
                 return Results.Problem("HttpContext is not available.");
             
             var userPermissions = httpContext.User.Claims
                 .Where(c => c.Type == AuthConstants.Permissions)
                 .Select(c => c.Value)
                 .ToList();
            
             if (userPermissions.Count == 0)
                 return Results.Unauthorized();

             var cachedJson = await cache.GetStringAsync(CacheKey, cancellationToken);
             if (cachedJson is not null)
             {
                 var cachedData = JsonSerializer.Deserialize<DictionaryResultDto>(cachedJson);
                 return Results.Ok(cachedData);
             }

             var now = DateTime.UtcNow;
             
             var data = await context.DictionarySettingsEntities
                 .Where(x =>
                     x.Id == id &&
                     x.StartDate <= now &&
                     x.EndDate >= now &&
                     x.Permissions.Any(p => userPermissions.Contains(p.Name)))
                 .Select(x => new DictionarySettingsDto
                 {
                     AvailableDictionaries = x.AvailableDictionaries,
                     Permissions = x.Permissions
                 })
                 .FirstOrDefaultAsync(cancellationToken);
             
             if (data == null)
                 return Results.Forbid();
             
             var availableProperties = await context.DictionaryEntities
                 .Where(d => data.AvailableDictionaries.Contains(d.Id))
                 .Select(d => new DictionaryEntityDto
                 {
                     Id = d.Id,
                     Title = d.Title
                 })
                 .ToListAsync(cancellationToken);

             var result = new DictionaryResultDto
             {
                 AvailableDictionaries = availableProperties,
                 Permissions = data.Permissions.Select(p => p.Name).ToList()
             };
             
             //cache for hour
             await cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(result),
                 new DistributedCacheEntryOptions
                 {
                     AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(CacheDuration)
                 }, cancellationToken);

             return Results.Ok(result);
             //super check if in DB changed (Delta nuget package)
        }).UseDelta<WebDbContext>();
        return app;
    }
}