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
        
        var permissions = new List<PermissionEntity>
        {
            new() { Id = Guid.NewGuid(), Name = "CanViewAnimal" },
            new() { Id = Guid.NewGuid(), Name = "CanEditAnimal" },
            new() { Id = Guid.NewGuid(), Name = "CanDeleteAnimal" }
        };
        
        var dictionaries = new List<DictionaryEntity>
        {
            new() { Id = Guid.NewGuid(), Title = "Animal" },
            new() { Id = Guid.NewGuid(), Title = "Plant" },
            new() { Id = Guid.NewGuid(), Title = "Vehicle" },
            new() { Id = Guid.NewGuid(), Title = "Employee" },
            new() { Id = Guid.NewGuid(), Title = "Product" }
        };
        var availableDictionaries = dictionaries.Select(d => d.Id).ToList();
        
        var settings = dictionaries.Select(dictionary => new DictionarySettingsEntity(
            Guid.NewGuid(), 
            dictionaryId: dictionary.Id,
            code: $"{dictionary.Title}Form",
            description: $"Форма для добавления {dictionary.Title.ToLower()}",
            startDate: DateTime.Parse("2025-05-01T00:00:00Z").ToUniversalTime(),
            endDate: DateTime.Parse("2025-12-31T23:59:59Z").ToUniversalTime(),
            availableDictionaries: availableDictionaries,
            permissions: permissions
        )).ToList();

        var animals = new List<Animal>()
        {
            new() { Id = Guid.NewGuid(), NameKk = "Қасқыр", NameRu = "Волк" },
            new() { Id = Guid.NewGuid(), NameKk = "Аю", NameRu = "Медведь" },
            new() { Id = Guid.NewGuid(), NameKk = "Түлкі", NameRu = "Лиса" }
        };

        var users = new List<UserEntity>
        {
            new() {
                Id = Guid.NewGuid(),
                Username = "admin",
                PasswordHash = "admin", 
                Email = "admin",
                Permissions =
                [
                    permissions.First(p => p.Name == "CanEditAnimal"), 
                    permissions.First(p => p.Name == "CanViewAnimal"),
                    permissions.First(p => p.Name == "CanDeleteAnimal")
                ]
            },
            new() {
                Id = Guid.NewGuid(),
                Username = "string",
                PasswordHash = "string",
                Email = "string",
                Permissions = [permissions.First(p => p.Name == "CanViewAnimal")]
            }
        };
        
        await context.PermissionEntities.AddRangeAsync(permissions);
        await context.DictionaryEntities.AddRangeAsync(dictionaries);
        await context.DictionarySettingsEntities.AddRangeAsync(settings);
        await context.Animals.AddRangeAsync(animals);
        await context.Users.AddRangeAsync(users); 
        
        await context.SaveChangesAsync();
    }
}
