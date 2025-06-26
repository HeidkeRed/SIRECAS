using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Helpers;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class PlanimetriaController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly IWebHostEnvironment _env;
        private readonly ActividadService _actividadService;

        public PlanimetriaController(Sirecas2Context context, IWebHostEnvironment env, ActividadService actividadService)
        {
            _context = context;
            _env = env;
            _actividadService = actividadService;
        }

        [HttpGet]
        public IActionResult Registrar_Planimetria(int idIdentificacion)
        {
            var vm = new PlanimetriaExistenteViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar_Planimetria(PlanimetriaExistenteViewModel model)
        {
            if (ModelState.IsValid && model.Archivo != null)
            {
                var extension = Path.GetExtension(model.Archivo.FileName);
                var fileName = $"{Guid.NewGuid()}{extension}";

                var carpetaDestino = Path.Combine(_env.WebRootPath, "planimetria");
                if (!Directory.Exists(carpetaDestino))
                    Directory.CreateDirectory(carpetaDestino);
                
                var rutaCompleta = Path.Combine(carpetaDestino, fileName);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await model.Archivo.CopyToAsync(stream);
                }

                var planimetria = new PlanimetriaExistente
                {
                    IdIdentificacion = model.IdIdentificacion,
                    TipoPlanimetria = model.TipoPlanimetria,
                    RutaArchivo = "/planimetria/" + fileName,
                    Observaciones = model.Observaciones
                };

                _context.PlanimetriaExistentes.Add(planimetria);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync("registró una planimetría existente", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPlanimetria(int IdPlanimetria, string TipoPlanimetria, string Observaciones)
        {
            // Buscar el registro de PlanimetriaExistente en la base de datos
            var planimetria = await _context.PlanimetriaExistentes.FindAsync(IdPlanimetria);

            if (planimetria != null)
            {
                // Actualizar los campos del registro
                planimetria.TipoPlanimetria = TipoPlanimetria;
                planimetria.Observaciones = Observaciones;

                // Guardar los cambios
                await _context.SaveChangesAsync();

                // Registrar actividad
                await _actividadService.RegistrarActividadAsync($"actualizó la planimetría (ID: {IdPlanimetria})", planimetria.IdIdentificacion);
                // Redirigir al resumen de Identificacion
                return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = planimetria.IdIdentificacion });
            }

            // Si no se encuentra el registro, redirigir con un mensaje de error
            TempData["ErrorMessage"] = "La planimetría no fue encontrada.";
            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = 0 });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(int idPlanimetria, int idIdentificacion)
        {
            var planimetria = await _context.PlanimetriaExistentes.FindAsync(idPlanimetria);
            if (planimetria != null)
            {
                _context.PlanimetriaExistentes.Remove(planimetria);
                await _context.SaveChangesAsync();

                // Registrar actividad (si tienes _actividadService)
                await _actividadService.RegistrarActividadAsync($"eliminó la planimetría (ID: {idPlanimetria})", idIdentificacion);
            }

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion });
        }




    }
}