using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Facturas
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
            Factura = new Factura();  // Inicializar
        }

        [BindProperty]
        public Factura Factura { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.MetodoPago)
                .FirstOrDefaultAsync(f => f.id_factura == id);

            if (factura == null)
            {
                return NotFound();
            }

            Factura = factura;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var factura = await _context.Facturas.FindAsync(Factura.id_factura);
            if (factura != null)
            {
                _context.Facturas.Remove(factura);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Factura eliminada exitosamente";
            }

            return RedirectToPage("./Index");
        }
    }
}