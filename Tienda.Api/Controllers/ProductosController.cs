using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tienda.Api.Models;
using Tienda.Api.Servicios;

namespace Tienda.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoServicio _servicio;

        public ProductosController(IProductoServicio servicio)
        {
            _servicio = servicio;
        }

        // GET /api/productos?nombre=mouse
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos([FromQuery] string? nombre)
        {
            var productos = await _servicio.ObtenerTodos(nombre);
            return Ok(new { mensaje = "Productos obtenidos correctamente", data = productos });
        }

        // GET /api/productos/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var producto = await _servicio.ObtenerPorId(id);
            if (producto == null) return NotFound(new { mensaje = $"Producto con ID {id} no encontrado" });

            return Ok(new { mensaje = "Producto obtenido correctamente", data = producto });
        }

        // POST /api/productos
        [HttpPost]
        public async Task<IActionResult> Crear(Producto producto)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "Datos de producto inválidos", errores = ModelState });

            var creado = await _servicio.Crear(producto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, new { mensaje = "Producto creado correctamente", data = creado });
        }

        // PUT /api/productos/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(int id, Producto producto)
        {
            if (!ModelState.IsValid) return BadRequest(new { mensaje = "Datos de producto inválidos", errores = ModelState });

            var ok = await _servicio.Actualizar(id, producto);
            if (!ok) return NotFound(new { mensaje = $"Producto con ID {id} no encontrado" });

            return Ok(new { mensaje = $"Producto con ID {id} actualizado correctamente" });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                var ok = await _servicio.Eliminar(id);
                if (!ok) return NotFound(new { mensaje = $"Producto con ID {id} no encontrado" });

                return Ok(new { mensaje = $"Producto con ID {id} eliminado correctamente" });
            }
            catch (DbUpdateException)
            {
                return Conflict(new { mensaje = "No se pudo eliminar el producto. Puede estar relacionado con pedidos existentes." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al eliminar el producto.", detalle = ex.Message });
            }
        }
    }
}
