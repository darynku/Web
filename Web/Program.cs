using Delta;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Web;
using Web.Contexts;
using Web.Contexts.Extensions;
using Web.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();

builder.AddJwtAuth();

builder.Services.AddDbContext<WebDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

await app.AddMigrations();
await app.SeedAsync();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

Console.WriteLine(Guid.NewGuid());

app.AddAnimalEndpoints()
    .AddDictionarySettingsEndpoints()
    .AddUserEndpoints()
    .AddDictionaryEndpoint();

app.Run();