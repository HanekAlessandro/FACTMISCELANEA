using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;

namespace FactMiscelanea.Pages.Productos
{
    public class GetPrecioModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GetPrecioModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return Content("0");
            }

            var precio = await _context.PreciosPromociones
                .Where(p => p.id_producto == id)
                .Select(p => p.precio_final)
                .FirstOrDefaultAsync();

            return Content(precio.ToString());
        }
    }
}