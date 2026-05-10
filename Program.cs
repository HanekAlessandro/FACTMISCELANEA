using FactMiscelanea.Data;
using FactMiscelanea.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddRazorPages();

builder.Services.AddScoped<ProductoService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Productos");
    return Task.CompletedTask;
});

app.MapRazorPages();

// =============================================
// INICIALIZACIÓN DE BASE DE DATOS
// =============================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    try
    {
        // Verificar si la base de datos existe y se puede conectar
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("✅ Conexión a base de datos establecida correctamente");
            
            // Marcar la migración como aplicada para evitar conflictos
            // Esto es necesario porque ya ejecutaste el script SQL manualmente
            dbContext.MarkMigrationAsApplied();
            
            // Verificar que las tablas principales existen
            var hayProductos = dbContext.Productos.Any();
            Console.WriteLine($"📊 Estado: { (hayProductos ? "Base de datos con datos" : "Base de datos lista para operar") }");
        }
        else
        {
            Console.WriteLine("⚠️ Advertencia: No se pudo conectar a la base de datos");
            Console.WriteLine("Verifica que:");
            Console.WriteLine("1. SQL Server esté ejecutándose");
            Console.WriteLine("2. La cadena de conexión en appsettings.json sea correcta");
            Console.WriteLine("3. La base de datos 'MiscelaneaMaster' exista");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error al inicializar la base de datos: {ex.Message}");
    }
}

app.Run();