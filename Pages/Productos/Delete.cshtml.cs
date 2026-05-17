using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Productos
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new Producto();

        public bool TieneRegistros { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.id_producto == id);

            if (producto == null)
            {
                return NotFound();
            }

            Producto = producto;

            // Verificar si tiene registros relacionados
            TieneRegistros = await _context.FacturaDetalles.AnyAsync(d => d.id_producto == id) ||
                             await _context.DetalleCompras.AnyAsync(d => d.id_producto == id) ||
                             await _context.Kardex.AnyAsync(k => k.id_producto == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var producto = await _context.Productos.FindAsync(Producto.id_producto);
            
            if (producto == null)
            {
                return NotFound();
            }

            // Verificar si tiene registros relacionados
            var tieneRegistros = await _context.FacturaDetalles.AnyAsync(d => d.id_producto == Producto.id_producto) ||
                                 await _context.DetalleCompras.AnyAsync(d => d.id_producto == Producto.id_producto) ||
                                 await _context.Kardex.AnyAsync(k => k.id_producto == Producto.id_producto);

            if (tieneRegistros)
            {
                // Desactivar el producto en lugar de eliminar
                producto.activo = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"El producto '{producto.nombre}' ha sido desactivado (tiene historial)";
            }
            else
            {
                // Eliminar precios primero
                var precios = await _context.PreciosPromociones
                    .Where(p => p.id_producto == Producto.id_producto)
                    .ToListAsync();
                if (precios.Any())
                {
                    _context.PreciosPromociones.RemoveRange(precios);
                }
                
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Producto '{producto.nombre}' eliminado exitosamente";
            }

            return RedirectToPage("./Index");
        }
    }
}