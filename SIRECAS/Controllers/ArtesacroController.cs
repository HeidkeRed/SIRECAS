using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Helpers;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class ArtesacroController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public ArtesacroController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }
        [HttpGet]
        [Autorizado(1, 2)]
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
        [Autorizado(1, 2)]
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

                // Registrar actividad usando el servicio
                await _actividadService.RegistrarActividadAsync("agregó un nuevo elemento del arte sacro", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }



        // Acción para editar un Elemento de Arte Sacro
        [HttpGet]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EditarElemento(int idElemento)
        {
            var elemento = await _context.ElementoArteSacros.FindAsync(idElemento);
            if (elemento == null)
            {
                return NotFound();
            }

            var model = new ElementoArteSacroViewModel
            {
                IdElemento = elemento.IdElemento,
                IdIdentificacion = elemento.IdIdentificacion,
                Seccion = elemento.Seccion,
                Subtipo = elemento.Subtipo,
                Nombre = elemento.Nombre,
                Descripcion = elemento.Descripcion,
                FechaRegistro = elemento.FechaRegistro
            };

            return View(model);
        }

        // Acción para actualizar un Elemento de Arte Sacro
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EditarElemento(ElementoArteSacroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var elemento = await _context.ElementoArteSacros.FindAsync(model.IdElemento);
                if (elemento == null)
                {
                    return NotFound();
                }

                elemento.Seccion = model.Seccion;
                elemento.Subtipo = model.Subtipo;
                elemento.Nombre = model.Nombre;
                elemento.Descripcion = model.Descripcion;
                elemento.FechaRegistro = model.FechaRegistro ?? DateTime.Now;

                _context.ElementoArteSacros.Update(elemento);
                await _context.SaveChangesAsync();

                var nombreUsuario = HttpContext.Session.GetString("NombreUsuario") ?? "Usuario desconocido";
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

                await _actividadService.RegistrarActividadAsync("edito un elemento del arte sacro", model.IdIdentificacion);

                return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarElemento(int idElemento)
        {
            var elemento = await _context.ElementoArteSacros
                .Include(e => e.ArteSacroFotos)
                .FirstOrDefaultAsync(e => e.IdElemento == idElemento);

            if (elemento == null)
            {
                return NotFound();
            }

            if (elemento.ArteSacroFotos != null && elemento.ArteSacroFotos.Any())
            {
                TempData["ErrorEliminar"] = "Para borrar este elemento primero hay que borrar sus fotos asociadas";
                return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = elemento.IdIdentificacion });
            }

            _context.ElementoArteSacros.Remove(elemento);

            // Registrar actividad usando el servicio
            await _actividadService.RegistrarActividadAsync($"eliminó el elemento '{elemento.Nombre}'", elemento.IdIdentificacion);

            await _context.SaveChangesAsync();

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = elemento.IdIdentificacion });
        }



        // Acción para editar las fotos asociadas a un Elemento de Arte Sacro
        [HttpGet]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EditarFotosDelElemento(int idElemento)
        {
            var elemento = await _context.ElementoArteSacros
                .Include(e => e.ArteSacroFotos) // Carga las fotos relacionadas
                .FirstOrDefaultAsync(e => e.IdElemento == idElemento);

            if (elemento == null)
            {
                return NotFound();
            }

            return View(elemento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> AgregarFotos(int IdElemento, IFormFile NuevaFoto, string Observacion)
        {
            var elemento = await _context.ElementoArteSacros.FindAsync(IdElemento);
            if (elemento == null)
            {
                return NotFound();
            }

            if (NuevaFoto != null && NuevaFoto.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fotos");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(NuevaFoto.FileName);
                var filePath = Path.Combine(path, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await NuevaFoto.CopyToAsync(stream);
                }

                var foto = new ArteSacroFoto
                {
                    IdElemento = elemento.IdElemento,
                    RutaArchivo = "/fotos/" + fileName,
                    Descripcion = Observacion
                };

                _context.ArteSacroFotos.Add(foto);

                // Registrar actividad con servicio
                await _actividadService.RegistrarActividadAsync(
                    $"agregó una foto al elemento '{elemento.Nombre}'",
                    elemento.IdIdentificacion);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("EditarFotosDelElemento", new { idElemento = elemento.IdElemento });
        }




        // Acción para eliminar una foto de un Elemento de Arte Sacro
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarFoto(int idFoto)
        {
            var foto = await _context.ArteSacroFotos.FindAsync(idFoto);
            if (foto == null)
            {
                return NotFound();
            }

            var elemento = await _context.ElementoArteSacros
                .FirstOrDefaultAsync(e => e.IdElemento == foto.IdElemento);

            if (elemento != null)
            {
                await _actividadService.RegistrarActividadAsync(
                    $"eliminó una foto del elemento '{elemento.Nombre}'",
                    elemento.IdIdentificacion);
            }

            _context.ArteSacroFotos.Remove(foto);
            await _context.SaveChangesAsync();

            return RedirectToAction("EditarFotosDelElemento", new { idElemento = foto.IdElemento });
        }



        // POST: ElementoArteSacro/EditarFoto
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EditarFoto(int IdFoto, string Observacion, IFormFile NuevaFoto)
        {
            var foto = await _context.ArteSacroFotos.FindAsync(IdFoto);
            if (foto == null) return NotFound();

            foto.Descripcion = Observacion;

            if (NuevaFoto != null && NuevaFoto.Length > 0)
            {
                // Borrar la foto anterior si existe
                if (!string.IsNullOrEmpty(foto.RutaArchivo))
                {
                    var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", foto.RutaArchivo.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(rutaAnterior))
                    {
                        System.IO.File.Delete(rutaAnterior);
                    }
                }

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "fotos");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(NuevaFoto.FileName);
                var ruta = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await NuevaFoto.CopyToAsync(stream);
                }

                foto.RutaArchivo = "/fotos/" + uniqueFileName;
            }

            _context.ArteSacroFotos.Update(foto);

            var elemento = await _context.ElementoArteSacros
                .FirstOrDefaultAsync(e => e.IdElemento == foto.IdElemento);

            if (elemento != null)
            {
                await _actividadService.RegistrarActividadAsync(
                    $"editó una foto del elemento '{elemento.Nombre}'",
                    elemento.IdIdentificacion);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EditarFotosDelElemento), new { idElemento = foto.IdElemento });
        }
    }
}
