using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class DatosHistoricoController : Controller
    {
        private readonly SirecasContext _context;

        public DatosHistoricoController(SirecasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_DatosHistorico(int idIdentificacion)
        {
            var vm = new DatosHistoricoViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_DatosHistorico(DatosHistoricoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entidad = new DatosHistorico
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    FechaEvento = model.FechaEvento
                };

                _context.DatosHistoricos.Add(entidad);
                await _context.SaveChangesAsync();

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}
