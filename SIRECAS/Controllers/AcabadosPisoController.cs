using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class AcabadosPisoController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public AcabadosPisoController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_AcabadosPiso(int idIdentificacion)
        {
            var model = new AcabadosPisoViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model); // Esta buscará Views/AcabadoPiso/Registrar_AcabadosPiso.cshtml
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_AcabadosPiso(AcabadosPisoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.AcabadosPisos
                    .AnyAsync(a => a.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de acabados de piso para esta identificación.");
                    return View(model);
                }

                var entidad = new AcabadosPiso
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Tierra = model.Tierra,
                    Mosaico = model.Mosaico,
                    PisoCeramico = model.PisoCeramico,
                    ConcretoPulido = model.ConcretoPulido,
                    OtrosAcabados = model.OtrosAcabados,
                    Observaciones = model.Observaciones
                };

                _context.AcabadosPisos.Add(entidad);
                await _context.SaveChangesAsync();

                // Registrar actividad usando el servicio
                await _actividadService.RegistrarActividadAsync("registró los acabados de piso", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Eliminar_AcabadosPiso(int idIdentificacion)
        {
            var entity = await _context.AcabadosPisos
                .FirstOrDefaultAsync(a => a.IdIdentificacion == idIdentificacion);

            if (entity == null)
            {
                return NotFound();
            }

            _context.AcabadosPisos.Remove(entity);
            await _context.SaveChangesAsync();

            // Registrar actividad usando el servicio
            await _actividadService.RegistrarActividadAsync("eliminó los acabados de piso", idIdentificacion);

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion });
        }

    }
}