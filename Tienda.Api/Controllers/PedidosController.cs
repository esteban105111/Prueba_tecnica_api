using Microsoft.AspNetCore.Mvc;
using Tienda.Api.Models;
using Tienda.Api.Models.DTOs;
using Tienda.Api.Servicios;
using Microsoft.EntityFrameworkCore;

namespace Tienda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoServicio _servicio;

        public PedidosController(IPedidoServicio servicio)
        {
            _servicio = servicio;
        }

        // GET /api/pedidos?pagina=1&tamPagina=10&estado=Pagado&cliente=Juan
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(
            [FromQuery] int pagina = 1,
            [FromQuery] int tamPagina = 10,
            [FromQuery] EstadoPedido? estado = null,
            [FromQuery] string? cliente = null)
        {
            var pedidos = await _servicio.ObtenerTodos(pagina, tamPagina, estado, cliente);
            return Ok(new { mensaje = "Pedidos obtenidos correctamente", data = pedidos });
        }

        // GET /api/pedidos/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var pedido = await _servicio.ObtenerPorId(id);
            if (pedido == null) return NotFound(new { mensaje = $"Pedido con ID {id} no encontrado" });

            return Ok(new { mensaje = "Pedido obtenido correctamente", data = pedido });
        }

        // POST /api/pedidos
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] PedidoCrearDto pedidoDto)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "Datos de pedido inválidos", errores = ModelState });

            try
            {
                var pedido = new Pedido
                {
                    NombreCliente = pedidoDto.NombreCliente,
                    CreadoEn = pedidoDto.CreadoEn ?? DateTime.UtcNow,
                    Estado = pedidoDto.Estado ?? EstadoPedido.Pendiente,
                    Items = pedidoDto.Items.Select(i => new ItemPedido
                    {
                        ProductoId = i.ProductoId,
                        Cantidad = i.Cantidad,
                        PrecioUnitario = i.PrecioUnitario ?? 0
                    }).ToList()
                };

                var creado = await _servicio.Crear(pedido);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, new { mensaje = "Pedido creado correctamente", data = creado });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensaje = "Error al crear pedido", detalle = ex.Message });
            }
        }

        // PUT /api/pedidos/{id}/status
        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> CambiarEstado(int id, [FromQuery] EstadoPedido estado)
        {
            var ok = await _servicio.CambiarEstado(id, estado);
            if (!ok) return NotFound(new { mensaje = $"Pedido con ID {id} no encontrado" });

            return Ok(new { mensaje = $"Estado del pedido con ID {id} actualizado correctamente a {estado}" });
        }

        // DELETE /api/pedidos/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var ok = await _servicio.Eliminar(id);
                if (!ok) return NotFound(new { mensaje = $"Pedido con ID {id} no encontrado" });

                return Ok(new { mensaje = $"Pedido con ID {id} eliminado correctamente" });
            }
            catch (DbUpdateException)
            {
                return Conflict(new { mensaje = "No se pudo eliminar el pedido. Puede tener items relacionados." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al eliminar el pedido.", detalle = ex.Message });
            }
        }
    }
}
