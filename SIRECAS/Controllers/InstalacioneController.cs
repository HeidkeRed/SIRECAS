using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class InstalacioneController : Controller
    {
        private readonly SirecasContext _context;

        public InstalacioneController(SirecasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Instalacione(int idIdentificacion)
        {
            var model = new InstalacioneViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Instalacione(InstalacioneViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.Instalaciones
                    .AnyAsync(i => i.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de instalaciones para esta identificación.");
                    return View(model);
                }

                var entidad = new Instalacione
                {
                    IdIdentificacion = model.IdIdentificacion,
                    ElectricaVisible = model.ElectricaVisible,
                    ElectricaOculta = model.ElectricaOculta,
                    ObservacionesElectrica = model.ObservacionesElectrica,
                    SanitariaVisible = model.SanitariaVisible,
                    SanitariaOculta = model.SanitariaOculta,
                    ObservacionesSanitaria = model.ObservacionesSanitaria,
                    HidraulicaVisible = model.HidraulicaVisible,
                    HidraulicaOculta = model.HidraulicaOculta,
                    ObservacionesHidraulica = model.ObservacionesHidraulica,
                    GasVisible = model.GasVisible,
                    GasOculta = model.GasOculta,
                    ObservacionesGas = model.ObservacionesGas,
                    TelefoniaVisible = model.TelefoniaVisible,
                    TelefoniaOculta = model.TelefoniaOculta,
                    ObservacionesTelefonia = model.ObservacionesTelefonia,
                    OtroVisible = model.OtroVisible,
                    OtroOculta = model.OtroOculta,
                    ObservacionesOtro = model.ObservacionesOtro
                };

                _context.Instalaciones.Add(entidad);
                await _context.SaveChangesAsync();

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}

