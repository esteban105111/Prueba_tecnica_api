using Tienda.Api.Models;

namespace Tienda.Api.Servicios
{
    public interface IProductoServicio
    {
        Task<List<Producto>> ObtenerTodos(string? nombre);
        Task<Producto?> ObtenerPorId(int id);
        Task<Producto> Crear(Producto producto);
        Task<bool> Actualizar(int id, Producto producto);
        Task<bool> Eliminar(int id);
    }
}
