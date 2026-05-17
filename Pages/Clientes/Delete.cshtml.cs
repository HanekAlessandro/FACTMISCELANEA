using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Clientes
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente cliente { get; set; } = new Cliente();
        
        public bool TieneFacturas { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clienteDb = await _context.Clientes.FindAsync(id);
            if (clienteDb == null)
            {
                return NotFound();
            }

            cliente = clienteDb;
            
            // Verificar si tiene facturas asociadas
            TieneFacturas = await _context.Facturas.AnyAsync(f => f.id_cliente == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var clienteExistente = await _context.Clientes.FindAsync(cliente.id_cliente);
            if (clienteExistente == null)
            {
                return NotFound();
            }

            // Verificar si tiene facturas
            var tieneFacturas = await _context.Facturas.AnyAsync(f => f.id_cliente == cliente.id_cliente);
            
            if (tieneFacturas)
            {
                // Opción 1: Desactivar el cliente
                clienteExistente.activo = false;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cliente desactivado correctamente (tiene facturas asociadas)";
            }
            else
            {
                // Eliminar físicamente
                _context.Clientes.Remove(clienteExistente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cliente eliminado correctamente";
            }

            return RedirectToPage("./Index");
        }
    }
}