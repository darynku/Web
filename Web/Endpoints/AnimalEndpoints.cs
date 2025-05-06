using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Web.Contexts;
using Web.Domain;

namespace Web.Endpoints;

public static class AnimalEndpoints 
{
    private const int CacheDuration = 5;
    private const string CacheKey = "animals_dic";
    public static void AddAnimalEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/dic/animals", async (
            [FromServices] WebDbContext context,
            [FromServices] IMemoryCache cache,
            CancellationToken cancellationToken) =>
        {
            if (cache.TryGetValue(CacheKey, out List<Animal>? cachedAnimals))
            {
                return Results.Ok(cachedAnimals);
            }
            var animals = await context.Animals.ToListAsync(cancellationToken);

            cache.Set(CacheKey, animals, TimeSpan.FromMinutes(CacheDuration));

            return Results.Ok(animals);
        }).WithName("GetAnimals")
        .WithDisplayName("Get animals");
    }
}
