using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Facturas
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
            // Inicializar propiedades para evitar CS8618/CS8601
            Factura = new Factura();
            DetallesFactura = new List<FacturaDetalle>();
        }

        public Factura Factura { get; set; }
        public List<FacturaDetalle> DetallesFactura { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Cargar la factura
            var factura = await _context.Facturas
                .Include(f => f.Cliente)
                .Include(f => f.MetodoPago)
                .Include(f => f.Usuario)
                .FirstOrDefaultAsync(f => f.id_factura == id);

            if (factura == null)
            {
                return NotFound();
            }

            // Asignar después de verificar que no es null
            Factura = factura;

            // Cargar los detalles de la factura por separado
            DetallesFactura = await _context.FacturaDetalles
                .Include(d => d.Producto)
                .Where(d => d.id_factura == id)
                .ToListAsync();

            return Page();
        }
    }
}