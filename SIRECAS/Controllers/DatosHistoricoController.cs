using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class DatosHistoricoController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public DatosHistoricoController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
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

                // Registrar la actividad usando el servicio
                await _actividadService.RegistrarActividadAsync("registró un dato histórico", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Actualizar_DatosHistorico(int IdDatoHistorico, int IdIdentificacion, string Titulo, string Descripcion, DateTime? FechaEvento)
        {
            if (ModelState.IsValid)
            {
                var dato = await _context.DatosHistoricos.FindAsync(IdDatoHistorico);

                if (dato != null)
                {
                    dato.Titulo = Titulo;
                    dato.Descripcion = Descripcion;
                    dato.FechaEvento = FechaEvento.HasValue ? DateOnly.FromDateTime(FechaEvento.Value) : null;

                    _context.Update(dato);
                    await _context.SaveChangesAsync();

                    // Registrar actividad usando el servicio
                    await _actividadService.RegistrarActividadAsync("actualizó un dato histórico", IdIdentificacion);

                    return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = IdIdentificacion });
                }
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Eliminar_DatosHistorico(int idDatoHistorico, int idIdentificacion)
        {
            var dato = await _context.DatosHistoricos.FindAsync(idDatoHistorico);
            if (dato != null)
            {
                _context.DatosHistoricos.Remove(dato);
                await _context.SaveChangesAsync();

                // Registrar actividad usando el servicio
                await _actividadService.RegistrarActividadAsync("eliminó un dato histórico", idIdentificacion);
            }

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion });
        }

    }
}