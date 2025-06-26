using Microsoft.AspNetCore.Mvc;
using SIRECAS.Helpers;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class TrabajoController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public TrabajoController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }

        [HttpGet]
        public IActionResult Registrar_Trabajo(int idIdentificacion)
        {
            var model = new RegistroTrabajoViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Trabajo(RegistroTrabajoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trabajo = new RegistroTrabajo
                {
                    IdIdentificacion = model.IdIdentificacion,
                    TipoRegistro = model.TipoRegistro,
                    Descripcion = model.Descripcion,
                    EmpresaResponsable = model.EmpresaResponsable,
                    FechaTrabajo = model.FechaTrabajo,
                    Observaciones = model.Observaciones
                };

                _context.RegistroTrabajos.Add(trabajo);
                await _context.SaveChangesAsync();

                // Registrar actividad
                await _actividadService.RegistrarActividadAsync("registró datos de trabajo", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> ActualizarTrabajo(int IdTrabajo, string TipoRegistro, string Descripcion, string EmpresaResponsable, DateTime? FechaTrabajo, string Observaciones)
        {
            var trabajo = await _context.RegistroTrabajos.FindAsync(IdTrabajo);
            if (trabajo != null)
            {
                trabajo.TipoRegistro = TipoRegistro;
                trabajo.Descripcion = Descripcion;
                trabajo.EmpresaResponsable = EmpresaResponsable;

                if (FechaTrabajo.HasValue)
                {
                    trabajo.FechaTrabajo = DateOnly.FromDateTime(FechaTrabajo.Value);
                }
                else
                {
                    trabajo.FechaTrabajo = null;
                }

                trabajo.Observaciones = Observaciones;

                await _context.SaveChangesAsync();

                // Registrar actividad
                await _actividadService.RegistrarActividadAsync("actualizó datos de trabajo", trabajo.IdIdentificacion);

                return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = trabajo?.IdIdentificacion });
            }

            TempData["ErrorMessage"] = "El trabajo no fue encontrado.";
            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = trabajo?.IdIdentificacion });
        }


        // Acción para eliminar el trabajo
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarTrabajo(int IdTrabajo)
        {
            var trabajo = await _context.RegistroTrabajos.FindAsync(IdTrabajo);
            if (trabajo != null)
            {
                _context.RegistroTrabajos.Remove(trabajo);
                await _context.SaveChangesAsync();

                // Registrar actividad
                await _actividadService.RegistrarActividadAsync("eliminó un trabajo", trabajo.IdIdentificacion);
            }

            // Redirigir a la acción Resumen con el parámetro idIdentificacion
            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = trabajo?.IdIdentificacion });
        }


    }
}