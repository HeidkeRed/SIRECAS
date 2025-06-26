using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class EntrepisoController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public EntrepisoController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Entrepiso(int idIdentificacion)
        {
            var model = new EntrepisoViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Entrepiso(EntrepisoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.Entrepisos
                    .AnyAsync(e => e.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de entrepisos para esta identificación.");
                    return View(model);
                }

                var entidad = new Entrepiso
                {
                    IdIdentificacion = model.IdIdentificacion,
                    LosaMaciza = model.LosaMaciza,
                    LosaReticular = model.LosaReticular,
                    ViguetaBovedilla = model.ViguetaBovedilla,
                    OtrosEntrepisos = model.OtrosEntrepisos,
                    NoAplica = model.NoAplica,
                    Observaciones = model.Observaciones
                };

                _context.Entrepisos.Add(entidad);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync("registró un entrepiso", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Eliminar_Entrepiso(int idIdentificacion)
        {
            var entidad = await _context.Entrepisos.FirstOrDefaultAsync(e => e.IdIdentificacion == idIdentificacion);
            if (entidad != null)
            {
                _context.Entrepisos.Remove(entidad);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync("eliminó un entrepiso", idIdentificacion);
            }

            return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion });
        }

    }
}