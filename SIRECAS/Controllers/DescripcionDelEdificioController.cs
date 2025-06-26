using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;
using Microsoft.EntityFrameworkCore;

namespace SIRECAS.Controllers
{
    public class DescripcionDelEdificioController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public DescripcionDelEdificioController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Descripcion(DescripcionDelEdificioViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe una descripción para esa identificación
                var yaExiste = await _context.DescripcionDelEdificios
                    .AnyAsync(d => d.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe una descripción registrada para esta identificación.");
                    // Devolver la vista con el modelo para mostrar el error
                    return View("Registrar_Descripcion", model);
                }

                var entidad = new DescripcionDelEdificio
                {
                    IdIdentificacion = model.IdIdentificacion,
                    DescripcionGeneral = model.DescripcionGeneral,
                    DescripcionFachada = model.DescripcionFachada
                };

                if (model.Foto != null && model.Foto.Length > 0)
                {
                    var nombreArchivo = Guid.NewGuid() + Path.GetExtension(model.Foto.FileName);
                    var ruta = Path.Combine("wwwroot/fotos", nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await model.Foto.CopyToAsync(stream);
                    }

                    entidad.FotografiaFachada = "/fotos/" + nombreArchivo;
                }

                _context.DescripcionDelEdificios.Add(entidad);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync($"agregó una descripción del edificio", model.IdIdentificacion);

                return RedirectToAction("Confirmacion", "Identificacion");
            }

            return View("Registrar_Descripcion", model);
        }


        [HttpGet]
        public IActionResult Registrar_Descripcion(int idIdentificacion)
        {
            var model = new DescripcionDelEdificioViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)] // Que solo administradores y editores puedan actualizar
        public async Task<IActionResult> ActualizarDescripcion(DescripcionDelEdificioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, regresa a la misma vista
                return View(model);
            }

            var entidad = await _context.DescripcionDelEdificios
                .FirstOrDefaultAsync(x => x.IdIdentificacion == model.IdIdentificacion);

            if (entidad == null)
            {
                return NotFound();
            }

            // Actualizar los campos permitidos
            entidad.DescripcionGeneral = model.DescripcionGeneral;
            entidad.DescripcionFachada = model.DescripcionFachada;

            // Si se cargó una nueva imagen
            if (model.Foto != null && model.Foto.Length > 0)
            {
                var nombreArchivo = Guid.NewGuid() + Path.GetExtension(model.Foto.FileName);
                var ruta = Path.Combine("wwwroot/fotos", nombreArchivo);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await model.Foto.CopyToAsync(stream);
                }

                entidad.FotografiaFachada = "/fotos/" + nombreArchivo;
            }

            // Guardar cambios
            _context.DescripcionDelEdificios.Update(entidad);
            await _context.SaveChangesAsync();
            await _actividadService.RegistrarActividadAsync($"actualizó la descripción del edificio", model.IdIdentificacion);

            TempData["SuccessMessage"] = "Descripción del edificio actualizada correctamente.";
            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarDescripcion([FromForm] int idIdentificacion)
        {
            if (idIdentificacion == 0)
                return BadRequest();

            var entidad = await _context.DescripcionDelEdificios
                .FirstOrDefaultAsync(x => x.IdIdentificacion == idIdentificacion);

            if (entidad == null)
                return NotFound();

            if (!string.IsNullOrEmpty(entidad.FotografiaFachada))
            {
                var ruta = Path.Combine("wwwroot", entidad.FotografiaFachada.TrimStart('/'));
                if (System.IO.File.Exists(ruta))
                    System.IO.File.Delete(ruta);
            }

            _context.DescripcionDelEdificios.Remove(entidad);
            await _context.SaveChangesAsync();

            await _actividadService.RegistrarActividadAsync($"eliminó la descripción del edificio", idIdentificacion);


            TempData["SuccessMessage"] = "Descripción eliminada correctamente.";
            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion });
        }




    }
}