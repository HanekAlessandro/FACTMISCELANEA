using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using FactMiscelanea.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FactMiscelanea.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegisterModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RegisterInputModel Input { get; set; } = new();

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") != null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Input.Password != Input.ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Las contraseñas no coinciden";
                return Page();
            }

            var existeUsuario = await _context.Usuarios
                .AnyAsync(u => u.nombre_usuario == Input.NombreUsuario);
            
            if (existeUsuario)
            {
                TempData["ErrorMessage"] = "El nombre de usuario ya existe";
                return Page();
            }

            var existeEmail = await _context.Usuarios
                .AnyAsync(u => u.email == Input.Email);
            
            if (existeEmail)
            {
                TempData["ErrorMessage"] = "El email ya está registrado";
                return Page();
            }

            var nuevoUsuario = new Usuario
            {
                nombre_completo = Input.NombreCompleto,
                email = Input.Email,
                nombre_usuario = Input.NombreUsuario,
                password_hash = HashPasswordToBytes(Input.Password),
                rol = "Cajero",
                estado = "Activo",
                intentos_fallidos = 0,
                fecha_registro = System.DateTime.Now,
                id_empresa = 1
            };

            _context.Usuarios.Add(nuevoUsuario);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cuenta creada exitosamente. Ahora puedes iniciar sesión.";
            return RedirectToPage("/Account/Login");
        }

        private byte[] HashPasswordToBytes(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            return sha256.ComputeHash(bytes);
        }
    }

    public class RegisterInputModel
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}