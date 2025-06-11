
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ProductsDb"));


builder.Services.AddCors(opt => opt.AddDefaultPolicy(
    p => p.WithOrigins("http://localhost:9000")
          .AllowAnyHeader()
          .AllowAnyMethod()
));

// 🔑  CORS
const string FrontOrigin = "FrontendDev";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: FrontOrigin, policy =>
    {
        policy
          .WithOrigins("http://localhost:9000")  // Quasar dev server
          .AllowAnyHeader()
          .AllowAnyMethod();
        // Si usas credenciales (cookies/Authorization):
        // .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger(); app.UseSwaggerUI();
app.UseCors();

app.MapGet("/api/products", async (AppDbContext db) => await db.Products.ToListAsync());

app.MapGet("/api/products/{id:int}", async (int id, AppDbContext db)
    => await db.Products.FindAsync(id) is Product p ? Results.Ok(p) : Results.NotFound());

app.MapPost("/api/products", async (Product p, AppDbContext db) =>
{
    db.Products.Add(p);
    await db.SaveChangesAsync();
    return Results.Created($"/api/products/{p.Id}", p);
});

app.MapPut("/api/products/{id:int}", async (int id, Product p, AppDbContext db) =>
{
    if (id != p.Id) return Results.BadRequest("Id mismatch");
    if (!await db.Products.AnyAsync(x => x.Id == id)) return Results.NotFound();
    db.Entry(p).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/products/{id:int}", async (int id, AppDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product is null) return Results.NotFound();
    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseCors();
app.UseCors(FrontOrigin);   // 👈 habilita la política


app.Run();