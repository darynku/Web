using Microsoft.EntityFrameworkCore;

namespace Web.Contexts.Extensions;

public static class MigrationExtension
{
    public static async Task AddMigrations(this IApplicationBuilder app)
    {
        await using var scope = app.ApplicationServices.CreateAsyncScope();

        var context = scope.ServiceProvider.GetRequiredService<WebDbContext>();
        
        await context.Database.MigrateAsync();
    }
}