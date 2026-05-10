using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Productos
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new Producto(); // Inicializado

        public SelectList? CategoriasList { get; set; } // Permitir nulo

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.id_producto == id);

            if (producto == null)
            {
                return NotFound();
            }

            Producto = producto;

            var categorias = await _context.Categorias.ToListAsync();
            CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria", Producto.id_categoria);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categorias = await _context.Categorias.ToListAsync();
                CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria", Producto.id_categoria);
                return Page();
            }

            try
            {
                var productoExistente = await _context.Productos
                    .FirstOrDefaultAsync(p => p.id_producto == Producto.id_producto);
                
                if (productoExistente == null)
                {
                    return NotFound();
                }

                productoExistente.codigo_barras = Producto.codigo_barras;
                productoExistente.nombre = Producto.nombre;
                productoExistente.descripcion = Producto.descripcion;
                productoExistente.id_categoria = Producto.id_categoria;
                productoExistente.stock_actual = Producto.stock_actual;
                productoExistente.stock_minimo = Producto.stock_minimo;
                productoExistente.iva_porcentaje = Producto.iva_porcentaje;

                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "✅ Producto actualizado exitosamente";
                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar: {ex.Message}");
                var categorias = await _context.Categorias.ToListAsync();
                CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria", Producto.id_categoria);
                return Page();
            }
        }
    }
}