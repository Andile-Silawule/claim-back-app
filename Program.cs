
using ClaimBack.API.Data;
using ClaimBack.API.Models;
using Microsoft.EntityFrameworkCore;
using ClaimBack.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<PurchaseService>();

// Register database context
builder.Services.AddDbContext<ClaimBackDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Register application services
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure HTTP requests
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.MapControllers();
app.Run();
