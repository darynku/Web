using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Web.Contexts;
using Web.Domain;

namespace Web.Endpoints;

public static class DictionarySettingsEndpoint
{
    private const string CacheKey = "availableProperties";
    private const int CacheDuration = 5;

    public static void AddDictionarySettingsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/dic/availableProperties/{id}", async (
            [FromServices] WebDbContext context,
            [FromServices] IMemoryCache cache,
            [FromRoute] Guid id,
            CancellationToken cancellationToken) =>
        {
            //IHttpContextAccessor httpContextAccessor jwt get permissions and check if available
            
            var now = DateTime.UtcNow;
            var data = await context.DictionarySettingsEntities
                .Where(x =>
                    x.StartDate <= now &&
                    x.EndDate >= now &&
                    x.Permissions.Any(p => p.Id == id))
                .Select(x => new
                {
                    x.Dictionary,
                    x.AvailableProperties,
                    x.Permissions
                })
                .FirstOrDefaultAsync(cancellationToken);
            
            //TODO check permissions is available
            
            //TODO reflection or something else to get properties from aviableProperties
            
            //TODO cache for hour
            
            //TODO super check if in DB changed (Delta nuget package)

            return Results.Ok(data);
        });
    }
}