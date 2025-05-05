using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Web.Contexts;
using Web.Domain;
using Web.Endpoints.Abstract;

namespace Web.Endpoints;

public class AnimalEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/animals", async (
            [FromServices] WebDbContext context,
            [FromServices] IMemoryCache cache,
            [FromQuery] string? nameKk,
            [FromQuery] string? nameRu,
            CancellationToken cancellationToken) =>
        {
            var cacheKey = $"animals:{nameKk}:{nameRu}";

            if (cache.TryGetValue(cacheKey, out List<Animal>? cachedAnimals))
            {
                return Results.Ok(cachedAnimals);
            }
            
            var query = context.Animals.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nameKk))
                query = query.Where(a => a.NameKk.Contains(nameKk));

            if (!string.IsNullOrWhiteSpace(nameRu))
                query = query.Where(a => a.NameRu.Contains(nameRu));

            var animals = await query.ToListAsync(cancellationToken);
            
            cache.Set(cacheKey, animals, TimeSpan.FromMinutes(5));

            return Results.Ok(animals);
        });
    }
}
