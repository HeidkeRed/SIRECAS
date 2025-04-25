using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class PlanimetriaController : Controller
    {
        private readonly SirecasContext _context;
        private readonly IWebHostEnvironment _env;

        public PlanimetriaController(SirecasContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}
