using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class CimientoController : Controller
    {
        private readonly SirecasContext _context;

        public CimientoController(SirecasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Cimiento(int idIdentificacion)
        {
            var vm = new CimientoViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Cimiento(CimientoViewModel model)
        {
            if (ModelState.IsValid)
            {
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

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}

