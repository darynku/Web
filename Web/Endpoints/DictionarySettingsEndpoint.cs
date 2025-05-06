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
        app.MapGet("api/dic/availableProperties/{code}", async (
            [FromServices] WebDbContext context,
            [FromServices] IMemoryCache cache,
            [FromRoute] string code,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            CancellationToken cancellationToken) =>
        {
            var data = await context.DictionarySettingsEntities
                .Where(x =>
                    x.StartDate <= endDate &&
                    x.EndDate >= startDate &&
                    x.Permissions.Any(p => p.Name == code))
                .Select(x => x.AvailableProperties)
                .ToListAsync(cancellationToken);

            return Results.Ok(data);
        });
    }
}