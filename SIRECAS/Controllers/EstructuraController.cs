using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class EstructuraController : Controller
    {
        private readonly SirecasContext _context;

        public EstructuraController(SirecasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Estructura(int idIdentificacion)
        {
            var vm = new EstructuraViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Estructura(EstructuraViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.Estructuras
                    .AnyAsync(e => e.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de estructura para esta identificación.");
                    return View(model);
                }

                var entidad = new Estructura
                {
                    IdIdentificacion = model.IdIdentificacion,
                    MuroPiedra = model.MuroPiedra,
                    MuroBlock = model.MuroBlock,
                    MuroLadrillo = model.MuroLadrillo,
                    MuroAdobe = model.MuroAdobe,
                    MuroBahareque = model.MuroBahareque,
                    OtroTipoMuro = model.OtroTipoMuro,
                    ColumnaConcreto = model.ColumnaConcreto,
                    ColumnaPiedra = model.ColumnaPiedra,
                    OtroTipoColumna = model.OtroTipoColumna,
                    Observaciones = model.Observaciones
                };

                _context.Estructuras.Add(entidad);
                await _context.SaveChangesAsync();

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}


