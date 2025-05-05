using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Web.Contexts;
using Web.Endpoints.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();

builder.Services.AddDbContext<WebDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapEndpoints();
app.UseHttpsRedirection();

app.Run();