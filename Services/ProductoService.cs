using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;

namespace FactMiscelanea.Services
{
    public class ProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ActualizarStock(string codigo, int cantidad)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC sp_UpdateStock @p0, @p1",
                codigo,
                cantidad
            );
        }
    }
}