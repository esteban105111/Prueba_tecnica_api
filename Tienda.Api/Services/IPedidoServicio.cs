using Tienda.Api.Models;

namespace Tienda.Api.Servicios
{
    public interface IPedidoServicio
    {
        // Obtener lista de pedidos con paginación, filtrado por estado y cliente
        Task<List<Pedido>> ObtenerTodos(int pagina, int tamPagina, EstadoPedido? estado, string? cliente);

        // Obtener un pedido por su ID
        Task<Pedido?> ObtenerPorId(int id);

        // Crear un nuevo pedido
        Task<Pedido> Crear(Pedido pedido);

        // Cambiar el estado de un pedido
        Task<bool> CambiarEstado(int id, EstadoPedido estado);

        // Eliminar un pedido
        Task<bool> Eliminar(int id);
    }
}
