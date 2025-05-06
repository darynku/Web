using Microsoft.EntityFrameworkCore;
using Web.Domain;

namespace Web.Contexts.Extensions;

public static class DbContextSeed
{
    public static async Task SeedAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WebDbContext>();
        
        if (await context.DictionaryEntities.AnyAsync()) return;

        var permission1 = new PermissionEntity
        {
            Id = Guid.NewGuid(),
            Name = "CanViewAnimalForm"
        };

        var permission2 = new PermissionEntity
        {
            Id = Guid.NewGuid(),
            Name = "CanEditAnimalForm"
        };

        var dictionary = new DictionaryEntity
        {
            Id = Guid.NewGuid(),
            Title= "Animal"
        };

        var setting = new DictionarySettingsEntity(
            dictionaryId: dictionary.Id,
            code: "formAnimal",
            description: "Форма для добавления животного",
            startDate: DateTime.Parse("2025-05-01T00:00:00Z").ToUniversalTime(),
            endDate: DateTime.Parse("2025-12-31T23:59:59Z").ToUniversalTime(),
            availableProperties: ["NameKk", "NameRu"],
            permissions: [permission1, permission2]
        )
        {
            Dictionary = dictionary
        };

        await context.PermissionEntities.AddRangeAsync(permission1, permission2);
        await context.DictionaryEntities.AddAsync(dictionary);
        await context.DictionarySettingsEntities.AddAsync(setting);

        await context.SaveChangesAsync();
    }

}