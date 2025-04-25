using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class ArtesacroController : Controller
    {
        private readonly SirecasContext _context;

        public ArtesacroController(SirecasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registro_Artesacro(int idIdentificacion)
        {
            var model = new ElementoArteSacroViewModel
            {
                IdIdentificacion = idIdentificacion,
                FechaRegistro = DateTime.Now
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro_Artesacro(
    ElementoArteSacroViewModel model,
    List<IFormFile> FotosSubidas,
    List<string> Descripciones)
        {
            if (ModelState.IsValid)
            {
                var elemento = new ElementoArteSacro
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Seccion = model.Seccion,
                    Subtipo = model.Subtipo,
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion,
                    FechaRegistro = model.FechaRegistro ?? DateTime.Now
                };

                _context.ElementoArteSacros.Add(elemento);
                await _context.SaveChangesAsync();

                if (FotosSubidas != null && FotosSubidas.Any())
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fotos");
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    for (int i = 0; i < FotosSubidas.Count; i++)
                    {
                        var file = FotosSubidas[i];
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(path, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        var descripcion = i < Descripciones.Count ? Descripciones[i] : null;

                        var foto = new ArteSacroFoto
                        {
                            IdElemento = elemento.IdElemento,
                            RutaArchivo = "/fotos/" + fileName,
                            Descripcion = descripcion
                        };

                        _context.ArteSacroFotos.Add(foto);
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }

    }
}
