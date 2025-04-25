using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class EntrepisoController : Controller
    {
        private readonly SirecasContext _context;

        public EntrepisoController(SirecasContext context)
        {
            _context = context;
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

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}
