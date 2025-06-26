using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class CubiertaController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public CubiertaController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Cubierta(int idIdentificacion)
        {
            var model = new CubiertaViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Cubierta(CubiertaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var yaExiste = await _context.Cubiertas
                    .AnyAsync(c => c.IdIdentificacion == model.IdIdentificacion);

                if (yaExiste)
                {
                    ModelState.AddModelError("", "Ya existe un registro de cubiertas para esta identificación.");
                    return View(model);
                }

                var entidad = new Cubierta
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Lamina = model.Lamina,
                    Concreto = model.Concreto,
                    Boveda = model.Boveda,
                    Cupula = model.Cupula,
                    Palma = model.Palma,
                    LosaPlana = model.LosaPlana,
                    DosAguas = model.DosAguas,
                    TresCuatroAguas = model.TresCuatroAguas,
                    OtrasCubiertas = model.OtrasCubiertas,
                    Observaciones = model.Observaciones
                };

                _context.Cubiertas.Add(entidad);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync("registró las cubiertas", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Eliminar_Cubierta(int idIdentificacion)
        {
            var entidad = await _context.Cubiertas.FirstOrDefaultAsync(c => c.IdIdentificacion == idIdentificacion);
            if (entidad != null)
            {
                _context.Cubiertas.Remove(entidad);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync("eliminó las cubiertas", idIdentificacion);
            }

            return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion });
        }


    }
}
