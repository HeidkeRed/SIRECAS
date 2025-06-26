using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class AcabadosMuroController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public AcabadosMuroController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }


        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_AcabadosMuros(int idIdentificacion)
        {
            var model = new AcabadosMuroViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_AcabadosMuros(AcabadosMuroViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.AcabadosMuros
                    .AnyAsync(m => m.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de acabados de muro para esta identificación.");
                    return View(model);
                }

                var entidad = new AcabadosMuro
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Aparente = model.Aparente,
                    BaseCemento = model.BaseCemento,
                    Yeso = model.Yeso,
                    Pintura = model.Pintura,
                    OtrosAcabados = model.OtrosAcabados,
                    Observaciones = model.Observaciones
                };

                _context.AcabadosMuros.Add(entidad);
                await _context.SaveChangesAsync();

                // Registrar actividad usando el servicio
                await _actividadService.RegistrarActividadAsync("registró los acabados de muros", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Eliminar_AcabadosMuros(int idIdentificacion)
        {
            var entidad = await _context.AcabadosMuros
                .FirstOrDefaultAsync(m => m.IdIdentificacion == idIdentificacion);

            if (entidad == null)
            {
                return NotFound();
            }

            _context.AcabadosMuros.Remove(entidad);
            await _context.SaveChangesAsync();

            // Usar el servicio para registrar la actividad
            await _actividadService.RegistrarActividadAsync("eliminó los acabados de muros", idIdentificacion);

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion });
        }

    }
}
