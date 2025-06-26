using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class DanoObservableController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly IWebHostEnvironment _env;
        private readonly ActividadService _actividadService;

        public DanoObservableController(Sirecas2Context context, IWebHostEnvironment env, ActividadService actividadService)
        {
            _context = context;
            _env = env;
            _actividadService = actividadService;
        }

        [HttpGet]
        [Autorizado(1, 2)]
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
        [Autorizado(1, 2)]
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

                // Registrar actividad usando el servicio
                await _actividadService.RegistrarActividadAsync("registró un daño observable", model.IdIdentificacion);

                

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> ActualizarDano(int IdDano, string Tipo, string Zona, string Estado, Identificacion model)
        {
            var dano = _context.DanoObservables.FirstOrDefault(d => d.IdDano == IdDano);
            if (dano != null)
            {
                dano.Tipo = Tipo;
                dano.Zona = Zona;
                dano.Estado = Estado;

                // Registrar actividad usando el servicio
                await _actividadService.RegistrarActividadAsync("actualizó un daño observable", model.IdIdentificacion);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EditarFotosDelDano(int idDano)
        {
            var dano = await _context.DanoObservables
                .Include(d => d.DanoFotos)
                .FirstOrDefaultAsync(d => d.IdDano == idDano);

            if (dano == null)
                return NotFound();

            return View(dano);
        }

        [HttpPost]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EditarFoto(int IdFoto, string Observacion, IFormFile? NuevaFoto)
        {
            var foto = await _context.DanoFotos.FindAsync(IdFoto);
            if (foto == null)
                return NotFound();

            // Actualizar solo la observación si no se carga una nueva foto
            foto.Observacion = Observacion;

            if (NuevaFoto != null)
            {
                // Eliminar el archivo antiguo
                var rutaVieja = Path.Combine(_env.WebRootPath, foto.RutaArchivo.TrimStart('/'));
                if (System.IO.File.Exists(rutaVieja))
                {
                    System.IO.File.Delete(rutaVieja);
                }

                // Generar nuevo nombre único para la foto
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(NuevaFoto.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "fotos", uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await NuevaFoto.CopyToAsync(stream);
                }

                foto.RutaArchivo = "/fotos/" + uniqueFileName;
            }

            _context.DanoFotos.Update(foto);
            await _context.SaveChangesAsync();

            // Registrar actividad con el servicio
            var dano = await _context.DanoObservables
                .FirstOrDefaultAsync(d => d.IdDano == foto.IdDano);

            if (dano != null)
            {
                await _actividadService.RegistrarActividadAsync("modificó una foto de un daño observable", dano.IdIdentificacion);
            }

            return RedirectToAction("EditarFotosDelDano", new { idDano = foto.IdDano });
        }



        [HttpPost]
        [Autorizado(1, 2)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarFoto(int idFoto)
        {
            var foto = await _context.DanoFotos.FindAsync(idFoto);
            if (foto == null)
                return NotFound();

            // Obtener el daño observable
            var dano = await _context.DanoObservables
                .FirstOrDefaultAsync(d => d.IdDano == foto.IdDano);

            if (dano == null)
                return NotFound();

            // Eliminar el archivo físico
            var ruta = Path.Combine(_env.WebRootPath, foto.RutaArchivo.TrimStart('/'));
            if (System.IO.File.Exists(ruta))
                System.IO.File.Delete(ruta);

            _context.DanoFotos.Remove(foto);
            await _context.SaveChangesAsync();

            // Registrar actividad con el servicio
            await _actividadService.RegistrarActividadAsync("eliminó una foto de un daño observable", dano.IdIdentificacion);

            return RedirectToAction("EditarFotosDelDano", new { idDano = foto.IdDano });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> AgregarFoto(int IdDano, IFormFile NuevaFoto, string Observacion)
        {
            if (NuevaFoto == null || NuevaFoto.Length == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar una foto para subir.");
                return RedirectToAction("EditarFotosDelDano", new { idDano = IdDano });
            }

            // Guardar la foto en disco
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(NuevaFoto.FileName);
            var filePath = Path.Combine(_env.WebRootPath, "fotos", uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await NuevaFoto.CopyToAsync(stream);
            }

            // Crear el nuevo registro de la foto
            var nuevaFoto = new DanoFoto
            {
                IdDano = IdDano,
                RutaArchivo = "/fotos/" + uniqueFileName,
                Observacion = Observacion
            };

            _context.DanoFotos.Add(nuevaFoto);
            await _context.SaveChangesAsync();

            // Obtener el IdIdentificacion desde el daño
            var dano = await _context.DanoObservables.FirstOrDefaultAsync(d => d.IdDano == IdDano);
            if (dano != null)
            {
                await _actividadService.RegistrarActividadAsync("agregó una foto de un daño observable", dano.IdIdentificacion);
            }

            return RedirectToAction("EditarFotosDelDano", new { idDano = IdDano });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarDano(int idDano)
        {
            var dano = await _context.DanoObservables
                .Include(d => d.DanoFotos)
                .Include(d => d.IdIdentificacionNavigation)
                .FirstOrDefaultAsync(d => d.IdDano == idDano);

            if (dano == null)
                return NotFound();

            if (dano.DanoFotos != null && dano.DanoFotos.Any())
            {
                TempData["ErrorEliminarDano"] = "Para poder eliminar este daño debes borrar primero las fotos asociadas.";
                return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = dano.IdIdentificacion });
            }

            int idIdentificacion = dano.IdIdentificacion;

            _context.DanoObservables.Remove(dano);
            await _context.SaveChangesAsync();

            await _actividadService.RegistrarActividadAsync("eliminó un daño observable", idIdentificacion);

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = idIdentificacion });
        }
    }
}