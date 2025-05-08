using System.Collections;
using Microsoft.EntityFrameworkCore;
using Web.Domain;

namespace Web.Contexts;

public class WebDbContext(DbContextOptions<WebDbContext> options) : DbContext(options)
{ 
    public DbSet<Animal> Animals { get; set; }
    public DbSet<DictionarySettingsEntity> DictionarySettingsEntities { get; set; }
    public DbSet<DictionaryEntity> DictionaryEntities { get; set; }
    public DbSet<PermissionEntity> PermissionEntities { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     modelBuilder.Entity<DictionarySettingsEntity>()
    //         .Property<byte[]>("Version")
    //         .IsRowVersion()
    //         .HasDefaultValue(new byte[] { 1 });
    // }
}