using Microsoft.EntityFrameworkCore;
using Tienda.Api.Data;
using Tienda.Api.Models;

namespace Tienda.Api.Servicios
{
    public class ProductoServicio : IProductoServicio
    {
        private readonly AppDbContext _context;

        public ProductoServicio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Producto>> ObtenerTodos(string? nombre)
        {
            var consulta = _context.Productos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
                consulta = consulta.Where(p => p.Nombre.Contains(nombre));

            return await consulta.ToListAsync();
        }

        public async Task<Producto?> ObtenerPorId(int id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<Producto> Crear(Producto producto)
        {
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> Actualizar(int id, Producto datos)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            producto.Nombre = datos.Nombre;
            producto.Precio = datos.Precio;
            producto.Stock = datos.Stock;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Eliminar(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
