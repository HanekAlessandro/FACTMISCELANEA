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
            // Inicializar propiedades en el constructor
            CategoriasList = new SelectList(new List<Categoria>(), "id_categoria", "nombre_categoria");
            ProveedoresList = new SelectList(new List<Proveedor>(), "id_proveedor", "nombre_empresa");
            Producto = new Producto();
        }

        [BindProperty]
        public Producto Producto { get; set; }

        [BindProperty]
        public decimal Precio { get; set; }

        public SelectList CategoriasList { get; set; }
        public SelectList ProveedoresList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            Producto = producto;

            // Obtener precio actual
            var precio = await _context.PreciosPromociones
                .Where(p => p.id_producto == id)
                .Select(p => p.precio_final)
                .FirstOrDefaultAsync();
            Precio = precio;

            await CargarListas();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await CargarListas();
                return Page();
            }

            var productoExistente = await _context.Productos.FindAsync(Producto.id_producto);
            if (productoExistente == null) return NotFound();

            productoExistente.codigo_barras = Producto.codigo_barras;
            productoExistente.nombre = Producto.nombre;
            productoExistente.descripcion = Producto.descripcion;
            productoExistente.id_categoria = Producto.id_categoria;
            productoExistente.stock_actual = Producto.stock_actual;
            productoExistente.stock_minimo = Producto.stock_minimo;
            productoExistente.iva_porcentaje = Producto.iva_porcentaje;

            // Actualizar precio
            var precioExistente = await _context.PreciosPromociones
                .FirstOrDefaultAsync(p => p.id_producto == Producto.id_producto);
            
            if (precioExistente != null)
            {
                precioExistente.precio_final = Precio;
                precioExistente.precio_sugerido = Precio;
            }
            else
            {
                _context.PreciosPromociones.Add(new PrecioPromocion
                {
                    id_producto = Producto.id_producto,
                    costo_compra = Precio,
                    precio_sugerido = Precio,
                    precio_final = Precio
                });
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Producto actualizado exitosamente";
            return RedirectToPage("./Index");
        }

        private async Task CargarListas()
        {
            var categorias = await _context.Categorias.ToListAsync();
            CategoriasList = new SelectList(categorias, "id_categoria", "nombre_categoria", Producto.id_categoria);

            var proveedores = await _context.Proveedores.ToListAsync();
            ProveedoresList = new SelectList(proveedores, "id_proveedor", "nombre_empresa", Producto.id_proveedor_preferente);
        }
    }
}