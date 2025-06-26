using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;
using Microsoft.EntityFrameworkCore;

namespace SIRECAS.Controllers
{
    public class InstalacioneController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly ActividadService _actividadService;

        public InstalacioneController(Sirecas2Context context, ActividadService actividadService)
        {
            _context = context;
            _actividadService = actividadService;
        }

        [HttpGet]
        [Autorizado(1, 2)]
        public IActionResult Registrar_Instalacione(int idIdentificacion)
        {
            var vm = new InstalacioneViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Instalacione(InstalacioneViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool yaExiste = model.Tipo != "Otro" && _context.Instalaciones.Any(i =>
                    i.IdIdentificacion == model.IdIdentificacion &&
                    i.Tipo == model.Tipo);

                if (yaExiste)
                {
                    ModelState.AddModelError(string.Empty, $"Ya se ha registrado una instalación del tipo {model.Tipo}.");
                    return View(model);
                }

                var entidad = new Instalacione
                {
                    IdIdentificacion = model.IdIdentificacion,
                    Tipo = model.Tipo,
                    Visible = model.Visible,
                    Oculta = model.Oculta,
                    Observaciones = model.Observaciones
                };

                _context.Instalaciones.Add(entidad);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync($"registró una instalación del tipo {model.Tipo}", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> ActualizarInstalacion(int Id, string Observaciones, bool Visible, bool Oculta, int IdIdentificacion)
        {
            var instalacionExistente = await _context.Instalaciones.FindAsync(Id);
            if (instalacionExistente == null)
            {
                return NotFound();
            }

            instalacionExistente.Observaciones = Observaciones;
            instalacionExistente.Visible = Visible;
            instalacionExistente.Oculta = Oculta;

            await _context.SaveChangesAsync();

            await _actividadService.RegistrarActividadAsync(
                $"actualizó la instalación de tipo '{instalacionExistente.Tipo}'",
                IdIdentificacion
            );

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = IdIdentificacion });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarInstalacion(int Id)
        {
            var instalacion = await _context.Instalaciones.FindAsync(Id);
            if (instalacion == null)
            {
                return NotFound();
            }

            _context.Instalaciones.Remove(instalacion);
            await _context.SaveChangesAsync();

            await _actividadService.RegistrarActividadAsync($"eliminó la instalación de tipo '{instalacion.Tipo}'", instalacion.IdIdentificacion);

            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = instalacion.IdIdentificacion });
        }


    }
}
