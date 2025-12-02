using System.ComponentModel.DataAnnotations;

namespace Tienda.Api.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Precio debe ser > 0")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock debe ser >= 0")]
        public int Stock { get; set; }

        // Propiedad de navegación para los ItemsPedidos asociados
        public List<ItemPedido> Items { get; set; } = new();
    }
}
