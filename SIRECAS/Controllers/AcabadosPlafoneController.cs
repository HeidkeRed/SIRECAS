using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class AcabadosPlafoneController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public AcabadosPlafoneController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_AcabadosPlafone(int idIdentificacion)
        {
            var model = new AcabadosPlafoneViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_AcabadosPlafone(AcabadosPlafoneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.AcabadosPlafones
                    .AnyAsync(p => p.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de acabados de plafones para esta identificación.");
                    return View(model);
                }

                var entidad = new AcabadosPlafone
                {
                    IdIdentificacion = model.IdIdentificacion,
                    AparenteSinAcabadoFinal = model.AparenteSinAcabadoFinal,
                    BaseCemento = model.BaseCemento,
                    BaseYeso = model.BaseYeso,
                    Pintura = model.Pintura,
                    OtrosAcabados = model.OtrosAcabados,
                    Observaciones = model.Observaciones
                };

                _context.AcabadosPlafones.Add(entidad);
                await _context.SaveChangesAsync();

                // Usar el servicio para registrar actividad
                await _actividadService.RegistrarActividadAsync("registró los acabados de plafones", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Eliminar_AcabadosPlafone(int idIdentificacion)
        {
            var entity = await _context.AcabadosPlafones
                .FirstOrDefaultAsync(a => a.IdIdentificacion == idIdentificacion);

            if (entity == null)
            {
                return NotFound();
            }

            _context.AcabadosPlafones.Remove(entity);
            await _context.SaveChangesAsync();

            // Registrar actividad con el servicio
            await _actividadService.RegistrarActividadAsync("eliminó los acabados de plafones", idIdentificacion);

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion });
        }

    }
}
