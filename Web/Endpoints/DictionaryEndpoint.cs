using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Contexts;
using Web.Domain;
using Web.Endpoints.Requests;

namespace Web.Endpoints;

public static class DictionaryEndpoint
{
    public static IEndpointRouteBuilder AddDictionaryEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("api/update/dictionary/{id:guid}", async (
            [FromRoute] Guid id,
            [FromBody] UpdateDictionaryDto updateDto,
            [FromServices] WebDbContext context,
            CancellationToken cancellationToken) =>
        {
            var entity = await context.DictionaryEntities
                .Where(d => d.Id == id)
                .ExecuteUpdateAsync(d => d.SetProperty(e => e.Title, updateDto.Title), cancellationToken);

            if (entity == 0) 
                return Results.NotFound();
            
            return Results.Ok(entity);
            
        }).RequireAuthorization(policy =>
        {
            policy.RequireRole(Role.Admin.ToString());
        });
        
        return app;
    }
}