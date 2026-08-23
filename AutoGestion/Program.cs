using AutoGestion.Data;
using AutoGestion.Extensions;
using Microsoft.AspNetCore.HttpOverrides; // <-- Requerido para el proxy de Fly.io
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Forwarded Headers para Fly.io
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Configuración de la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Registra automáticamente todos los repositorios del proyecto
builder.Services.AddRepositoriesAuto();

// Configuración de ASP.NET Core Identity
builder.Services.AddDefaultIdentity<AutoGestion.Data.ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>();

// Protección de Razor Pages
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToFolder("/Identity");
});

var app = builder.Build();

// 1. Asegurar que la carpeta para SQLite exista en el servidor (Fly.io)
if (!app.Environment.IsDevelopment())
{
    var dataDirectory = "/app/data";
    if (!Directory.Exists(dataDirectory))
    {
        Directory.CreateDirectory(dataDirectory);
    }
}

// Procesa el tráfico HTTPS del proxy inverso de Fly.io antes de cualquier otra regla
app.UseForwardedHeaders();

// 2. fAplicar migraciones automáticas e inicializar catálogos
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // 1. Aplica migraciones / crea tablas
    dbContext.Database.Migrate();

    // 2. Poblar catálogos si están vacíos
    DbInitializer.Seed(dbContext);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Middlewares de Seguridad
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();