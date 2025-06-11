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

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ProductsDb"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(FrontOrigin);

app.UseAuthorization();
app.MapControllers(); // <-- Usar controladores

app.Run();
