using System.ComponentModel.DataAnnotations;

namespace Tienda.Api.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public string NombreCliente { get; set; } = null!;

        public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

        public EstadoPedido Estado { get; set; } = EstadoPedido.Pendiente;

        public ICollection<ItemPedido> Items { get; set; } = new List<ItemPedido>();
    }
}
