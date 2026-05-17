using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;

namespace FactMiscelanea.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Endpoint para obtener información del producto (precio y stock)
        [HttpGet("ProductoInfo/{id}")]
        public async Task<IActionResult> GetProductoInfo(int id)
        {
            try
            {
                var producto = await _context.Productos
                    .Where(p => p.id_producto == id)
                    .Select(p => new
                    {
                        precio = _context.PreciosPromociones
                            .Where(pr => pr.id_producto == p.id_producto)
                            .Select(pr => pr.precio_final)
                            .FirstOrDefault(),
                        stock = p.stock_actual,
                        nombre = p.nombre
                    })
                    .FirstOrDefaultAsync();

                if (producto == null)
                    return Ok(new { error = true, message = "Producto no encontrado" });

                if (producto.precio <= 0)
                    return Ok(new { error = true, message = "El producto no tiene un precio configurado" });

                return Ok(new { error = false, precio = producto.precio, stock = producto.stock, nombre = producto.nombre });
            }
            catch (Exception ex)
            {
                return Ok(new { error = true, message = ex.Message });
            }
        }

        // Endpoint para obtener todos los productos (opcional)
        [HttpGet("Productos")]
        public async Task<IActionResult> GetProductos()
        {
            var productos = await _context.Productos
                .Where(p => p.activo)
                .Select(p => new
                {
                    p.id_producto,
                    p.nombre,
                    p.codigo_barras,
                    p.stock_actual,
                    precio = _context.PreciosPromociones
                        .Where(pr => pr.id_producto == p.id_producto)
                        .Select(pr => pr.precio_final)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Ok(productos);
        }
    }
}