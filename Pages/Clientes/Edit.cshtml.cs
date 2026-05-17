using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Clientes
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Cliente cliente { get; set; } = new Cliente();

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var clienteExistente = await _context.Clientes.FindAsync(cliente.id_cliente);
            if (clienteExistente == null)
            {
                return NotFound();
            }

            clienteExistente.identificacion = cliente.identificacion;
            clienteExistente.nombre = cliente.nombre;
            clienteExistente.telefono = cliente.telefono;
            clienteExistente.email = cliente.email;
            clienteExistente.direccion = cliente.direccion;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cliente actualizado correctamente";
            return RedirectToPage("./Index");
        }
    }
}