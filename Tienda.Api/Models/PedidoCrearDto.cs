// PedidoCrearDto.cs
using System.ComponentModel.DataAnnotations;

namespace Tienda.Api.Models.DTOs
{
    public class PedidoCrearDto
    {
        [Required]
        public string NombreCliente { get; set; } = null!;

        public DateTime? CreadoEn { get; set; } // opcional, puede generarse en el servidor

        public EstadoPedido? Estado { get; set; } = EstadoPedido.Pendiente;

        [Required]
        public List<ItemPedidoCrearDto> Items { get; set; } = new();
    }

    public class ItemPedidoCrearDto
    {
        [Required]
        public int ProductoId { get; set; }

        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        public decimal? PrecioUnitario { get; set; } // se llenará automáticamente
    }
}
