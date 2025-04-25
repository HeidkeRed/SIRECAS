using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class DanoObservableController : Controller
    {
        private readonly SirecasContext _context;
        private readonly IWebHostEnvironment _env;

        public DanoObservableController(SirecasContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Registro_Dano(int idIdentificacion)
        {
            var model = new DanoObservableViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro_Dano(DanoObservableViewModel model, List<IFormFile> FotosSubidas, List<string> Observaciones)
        {
            if (ModelState.IsValid)
            {
                var dano = new DanoObservable
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Tipo = model.Tipo,
                    Zona = model.Zona,
                    Estado = model.Estado
                };

                _context.DanoObservables.Add(dano);
                await _context.SaveChangesAsync();

                for (int i = 0; i < FotosSubidas.Count; i++)
                {
                    var file = FotosSubidas[i];
                    if (file != null && file.Length > 0)
                    {
                        var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                        var ruta = Path.Combine("wwwroot/fotos", nombreArchivo);

                        using (var stream = new FileStream(ruta, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        _context.DanoFotos.Add(new DanoFoto
                        {
                            IdDano = dano.IdDano,
                            RutaArchivo = "/fotos/" + nombreArchivo,
                            Observacion = i < Observaciones.Count ? Observaciones[i] : null
                        });
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }

    }
}

