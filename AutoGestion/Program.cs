using AutoGestion.Data;
using AutoGestion.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;

// Habilita el comportamiento legacy de timestamps para Npgsql (vital para SQLite -> PostgreSQL)
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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

// MODIFICACIÓN CLAVE: Cambiamos UseSqlite por UseNpgsql para PostgreSQL (Neon DB) y agregamos Ignore para PendingModelChangesWarning
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
           .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

// Registra automáticamente todos los repositorios y servicios del proyecto
builder.Services.AddRepositoriesAuto();
builder.Services.AddServicesAuto();

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

// Limitar la expiración de la cookie a 1 día máximo
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true; // Renueva el día si el usuario sigue activo
});

// Configurar Data Protection para producción (en el volumen persistente)
if (!builder.Environment.IsDevelopment())
{
    builder.Services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(@"/app/data/keys"));
}

// Protección de Razor Pages
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToFolder("/Identity");
});

var app = builder.Build();

// Asegurar que la carpeta de llaves de seguridad exista en el servidor (Fly.io)
if (!app.Environment.IsDevelopment())
{
    var keysDirectory = "/app/data/keys";
    if (!Directory.Exists(keysDirectory))
    {
        Directory.CreateDirectory(keysDirectory);
    }
}

// 1. PRIMERO DE TODO: Procesa el tráfico y cabeceras HTTPS del proxy inverso de Fly.io
app.UseForwardedHeaders();

// 2. Aplicar migraciones automáticas e inicializar catálogos SOLO EN PRODUCCIÓN
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // 1. Aplicar migraciones automáticas SOLO EN PRODUCCIÓN (o déjalo condicionado)
    if (!app.Environment.IsDevelopment())
    {
        dbContext.Database.Migrate();
    }

    // 2. Poblar catálogos en cualquier entorno (Desarrollo y Producción)
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