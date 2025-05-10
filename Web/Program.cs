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

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (app.Environment.IsEnvironment("Docker"))
{
    app.MapOpenApi();
    app.MapScalarApiReference().AllowAnonymous();
}

await app.AddMigrations();
await app.SeedAsync();

app.UseAuthentication();
app.UseAuthorization();


app.AddAnimalEndpoints()
    .AddDictionarySettingsEndpoints()
    .AddUserEndpoints()
    .AddDictionaryEndpoint();

app.UseHttpsRedirection();
app.Run();