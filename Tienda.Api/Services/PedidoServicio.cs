using Microsoft.EntityFrameworkCore;
using Tienda.Api.Data;
using Tienda.Api.Models;

namespace Tienda.Api.Servicios
{
    public class PedidoServicio : IPedidoServicio
    {
        private readonly AppDbContext _context;

        public PedidoServicio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Pedido>> ObtenerTodos(int pagina, int tamPagina, EstadoPedido? estado, string? cliente)
        {
            var consulta = _context.Pedidos
                .Include(p => p.Items)
                .ThenInclude(i => i.Producto)
                .AsQueryable();

            if (estado.HasValue)
                consulta = consulta.Where(p => p.Estado == estado);

            if (!string.IsNullOrWhiteSpace(cliente))
                consulta = consulta.Where(p => p.NombreCliente.Contains(cliente));

            return await consulta
                .Skip((pagina - 1) * tamPagina)
                .Take(tamPagina)
                .ToListAsync();
        }

        public async Task<Pedido?> ObtenerPorId(int id)
        {
            return await _context.Pedidos
                .Include(p => p.Items)
                .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pedido> Crear(Pedido pedido)
        {
            // Validación de items
            foreach (var item in pedido.Items)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto == null)
                    throw new Exception($"El producto con ID {item.ProductoId} no existe.");

                if (producto.Stock < item.Cantidad)
                    throw new Exception($"Stock insuficiente para el producto {producto.Nombre}.");

                item.PrecioUnitario = producto.Precio;

                // Descontar stock
                producto.Stock -= item.Cantidad;
            }

            pedido.CreadoEn = DateTime.UtcNow;
            pedido.Estado = EstadoPedido.Pendiente;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<bool> CambiarEstado(int id, EstadoPedido estado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return false;

            pedido.Estado = estado;
            await _context.SaveChangesAsync();
            return true;    
        }

        public async Task<bool> Eliminar(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Items) // incluir items para eliminar en cascada
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null) return false;

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
