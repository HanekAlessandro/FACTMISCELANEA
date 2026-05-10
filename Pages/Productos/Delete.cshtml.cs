using FactMiscelanea.Data;
using FactMiscelanea.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        public Producto Producto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            Producto = producto;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var producto = await _context.Productos.FindAsync(Producto.id_producto);

            if (producto != null)
            {
                _context.Productos.Remove(producto);

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("Index");
        }
    }
}