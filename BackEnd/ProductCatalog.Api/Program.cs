using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS
const string FrontOrigin = "FrontendDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: FrontOrigin, policy =>
    {
        policy
          .WithOrigins("http://localhost:9000")
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

const string DeployOrigin = "DeployDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: FrontOrigin, policy =>
    {
        policy
          .WithOrigins("https://code.jgonzalezfals.dev")
          .AllowAnyHeader()
          .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ProductsDb"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(FrontOrigin);
app.UseCors(DeployOrigin);
app.UseAuthorization();
app.MapControllers(); // <-- Usar controladores

app.Run();
