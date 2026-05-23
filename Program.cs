using Microsoft.EntityFrameworkCore;
using FactMiscelanea.Data;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// servicios
builder.Services.AddRazorPages();

// dbcontext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    )
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors()
);

// session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// entorno
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

app.UseSession();

app.UseAuthorization();

// middleware login
app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    var publicPaths = new[]
    {
        "/Account/Login",
        "/Account/Register",
        "/Account/Logout"
    };

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

app.MapRazorPages();

// probar conexion
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ApplicationDbContext>();

    try
    {
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("Base de datos conectada exitosamente");
        }
        else
        {
            Console.WriteLine("No se pudo conectar a la base de datos");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error de conexion: {ex.Message}");
    }
}

app.Run();