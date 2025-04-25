using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class AcabadosPlafoneController : Controller
    {
        private readonly SirecasContext _context;

        public AcabadosPlafoneController(SirecasContext context)
        {
            _context = context;
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

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}
