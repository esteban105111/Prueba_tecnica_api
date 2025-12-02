using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Tienda.Api.Data;
using Tienda.Api.Servicios;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 1. DbContext con SQL Server
// ============================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// ============================================
// 2. Servicios (capa de negocio)
// ============================================
builder.Services.AddScoped<IProductoServicio, ProductoServicio>();
builder.Services.AddScoped<IPedidoServicio, PedidoServicio>();

// ============================================
// 3. Controllers y Swagger con configuración JSON
// ============================================
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Ignorar ciclos de referencia al serializar
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // Salida indentada opcional
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ============================================
// 4. Pipeline
// ============================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
