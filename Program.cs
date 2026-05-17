using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Configurar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors());

// Agregar servicios de sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

// Middleware de autorización simple (alternativa)
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var publicPaths = new[] { "/Account/Login", "/Account/Register", "/Account/Logout" };
    
    var isPublic = publicPaths.Any(p => path.StartsWithSegments(p));
    
    if (!isPublic)
    {
        var userId = context.Session.GetInt32("UsuarioId");
        if (userId == null)
        {
            context.Response.Redirect("/Account/Login");
            return;
        }
    }
    
    await next();
});

// Probar conexión a BD
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("✅ Base de datos conectada exitosamente");
        }
        else
        {
            Console.WriteLine("⚠️ No se pudo conectar a la base de datos");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error de conexión: {ex.Message}");
    }
}

app.Run();