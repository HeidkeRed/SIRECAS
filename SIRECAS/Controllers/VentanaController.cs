using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class VentanaController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public VentanaController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }
        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Ventanas(int idIdentificacion)
        {
            var model = new VentanaViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model); // busca Views/Ventana/Registrar_Ventanas.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Ventanas(VentanaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.Ventanas
                    .AnyAsync(v => v.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de ventanas para esta identificación.");
                    return View(model);
                }

                var entidad = new Ventana
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Madera = model.Madera,
                    Herreria = model.Herreria,
                    Aluminio = model.Aluminio,
                    OtrosMateriales = model.OtrosMateriales,
                    Observaciones = model.Observaciones
                };

                _context.Ventanas.Add(entidad);
                await _context.SaveChangesAsync();

                // Registrar actividad
                await _actividadService.RegistrarActividadAsync("registró datos de ventanas", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Eliminar_Ventanas(int idIdentificacion)
        {
            var entidad = await _context.Ventanas.FirstOrDefaultAsync(v => v.IdIdentificacion == idIdentificacion);
            if (entidad != null)
            {
                _context.Ventanas.Remove(entidad);
                await _context.SaveChangesAsync();

                // Registrar actividad: cambió el mensaje para reflejar eliminación
                await _actividadService.RegistrarActividadAsync("eliminó datos de ventanas", idIdentificacion);
            }

            return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion });
        }


    }
}

