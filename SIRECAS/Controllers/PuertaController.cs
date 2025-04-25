using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class PuertaController : Controller
    {
        private readonly SirecasContext _context;

        public PuertaController(SirecasContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Puertas(int idIdentificacion)
        {
            var model = new PuertaViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model); // Buscará: Views/Puertas/Registrar_Puertas.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Puertas(PuertaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.Puertas
                    .AnyAsync(p => p.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de puertas para esta identificación.");
                    return View(model);
                }

                var entidad = new Puerta
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Madera = model.Madera,
                    Herreria = model.Herreria,
                    Aluminio = model.Aluminio,
                    OtrosMateriales = model.OtrosMateriales,
                    Observaciones = model.Observaciones
                };

                _context.Puertas.Add(entidad);
                await _context.SaveChangesAsync();

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}
