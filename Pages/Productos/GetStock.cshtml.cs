using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;

namespace FactMiscelanea.Pages.Productos
{
    public class GetStockModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public GetStockModel(ApplicationDbContext context)
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
            return Content(producto.stock_actual.ToString());
        }
    }
}