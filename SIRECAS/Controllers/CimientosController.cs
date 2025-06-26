using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;
using Microsoft.EntityFrameworkCore;

namespace SIRECAS.Controllers
{
    public class CimientosController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public CimientosController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Cimiento(int idIdentificacion)
        {
            var vm = new CimientosViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Cimientos(CimientosViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.Cimientos
                    .AnyAsync(c => c.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de Cimientos para esta identificación.");
                    // Aquí indico que la vista a mostrar es la de la acción GET
                    return View("Registrar_Cimiento", model);
                }

                var entidad = new Cimiento
                {
                    IdIdentificacion = model.IdIdentificacion,
                    TierraCompactada = model.TierraCompactada,
                    MamposteriaPiedra = model.MamposteriaPiedra,
                    ZapatasAisladas = model.ZapatasAisladas,
                    ZapatasCorridas = model.ZapatasCorridas,
                    LosaCimentacion = model.LosaCimentacion,
                    OtrosCimientos = model.OtrosCimientos,
                    Observaciones = model.Observaciones
                };

                _context.Cimientos.Add(entidad);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync("registró los Cimientos", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View("Registrar_Cimiento", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarCimientos(int idIdentificacion)
        {
            var Cimientos = await _context.Cimientos
                .FirstOrDefaultAsync(c => c.IdIdentificacion == idIdentificacion);

            if (Cimientos != null)
            {
                _context.Cimientos.Remove(Cimientos);
                await _context.SaveChangesAsync();

                // Registrar actividad usando el servicio
                await _actividadService.RegistrarActividadAsync("eliminó los Cimientos", idIdentificacion);
            }

            return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion });
        }


    }
}