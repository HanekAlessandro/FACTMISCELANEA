using FactMiscelanea.Data;
using FactMiscelanea.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FactMiscelanea.Pages.Productos
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Producto Producto { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.id_producto == id);

            if (producto == null)
            {
                return NotFound();
            }

            Producto = producto;

            return Page();
        }
    }
}