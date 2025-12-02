using System.ComponentModel.DataAnnotations;

namespace Tienda.Api.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Cantidad debe ser > 0")]
        public int Cantidad { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "PrecioUnitario debe ser > 0")]
        public decimal PrecioUnitario { get; set; }

        public int PedidoId { get; set; }
        public Pedido? Pedido { get; set; }
    }
}
