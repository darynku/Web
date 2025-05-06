using Microsoft.EntityFrameworkCore;
using Web.Domain;

namespace Web.Contexts;

public class WebDbContext(DbContextOptions<WebDbContext> options) : DbContext(options)
{ 
    public DbSet<Animal> Animals { get; set; }
    public DbSet<DictionarySettingsEntity> DictionarySettingsEntities { get; set; }
    public DbSet<DictionaryEntity> DictionaryEntities { get; set; }
    public DbSet<PermissionEntity> PermissionEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>().HasData(
            new Animal
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                NameKk = "Қасқыр",
                NameRu = "Волк"
            },
            new Animal
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                NameKk = "Аю",
                NameRu = "Медведь"
            },
            new Animal
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                NameKk = "Түлкі",
                NameRu = "Лиса"
            }
        );
    }
}