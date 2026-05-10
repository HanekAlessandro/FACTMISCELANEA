using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.id_producto == id);

            if (producto == null)
            {
                return NotFound();
            }

            Producto = producto;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var producto = await _context.Productos.FindAsync(id);

                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "✅ Producto eliminado exitosamente";
                }
                else
                {
                    TempData["ErrorMessage"] = "❌ Producto no encontrado";
                }
            }
            catch (DbUpdateException)
            {
                // Error por violación de clave foránea (no usamos la variable ex)
                TempData["ErrorMessage"] = "❌ No se puede eliminar el producto porque tiene registros relacionados (ventas, kardex, etc.)";
            }
            catch (System.Exception)
            {
                // Error genérico (no usamos la variable ex)
                TempData["ErrorMessage"] = "❌ Error al eliminar el producto";
            }

            return RedirectToPage("./Index");
        }
    }
}