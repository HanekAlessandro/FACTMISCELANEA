using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Productos
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new Producto(); // Inicializado

        public SelectList? CategoriasList { get; set; } // Permitir nulo

        public async Task<IActionResult> OnGetAsync()
        {
            var categorias = await _context.Categorias.ToListAsync();
            CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categorias = await _context.Categorias.ToListAsync();
                CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria");
                return Page();
            }

            try
            {
                _context.Productos.Add(Producto);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "✅ Producto creado exitosamente";
                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar: {ex.Message}");
                var categorias = await _context.Categorias.ToListAsync();
                CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria");
                return Page();
            }
        }
    }
}