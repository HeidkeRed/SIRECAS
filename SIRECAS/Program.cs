using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Agregar contexto de base de datos
builder.Services.AddDbContext<SirecasContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SirecasContext"));
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_073_741_824;
}
);

//Subir archivos mas grandes
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 1_073_741_824; 
});


// Agregar soporte para sesiones
builder.Services.AddSession(); // ? Esto va aquí, antes de Build

var app = builder.Build();

// Middleware del pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
provider.Mappings[".blend"] = "application/octet-stream"; // o "application/x-blender"

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});

app.UseStaticFiles();

app.UseRouting();

app.UseSession();         // ? Obligatorio antes de Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

