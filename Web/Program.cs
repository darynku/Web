using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Web.Contexts;
using Web.Contexts.Extensions;
using Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<WebDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.AddMigrations();
await app.SeedAsync();

app.UseHttpsRedirection();

app.AddAnimalEndpoints();
app.AddDictionarySettingsEndpoints();

app.Run();