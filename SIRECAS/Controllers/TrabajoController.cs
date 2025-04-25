using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class TrabajoController : Controller
    {
        private readonly SirecasContext _context;

        public TrabajoController(SirecasContext context)
        {
            _context = context;
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

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}
