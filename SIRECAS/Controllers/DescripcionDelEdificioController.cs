using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class DescripcionDelEdificioController : Controller
    {
        private readonly SirecasContext _context;

        public DescripcionDelEdificioController(SirecasContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> Registrar_Descripcion(DescripcionDelEdificioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entidad = new DescripcionDelEdificio
                {
                    IdIdentificacion = model.IdIdentificacion,
                    DescripcionGeneral = model.DescripcionGeneral,
                    DescripcionFachada = model.DescripcionFachada
                };

                // Guardar imagen
                if (model.Foto != null && model.Foto.Length > 0)
                {
                    var nombreArchivo = Guid.NewGuid() + Path.GetExtension(model.Foto.FileName);
                    var ruta = Path.Combine("wwwroot/fotos", nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await model.Foto.CopyToAsync(stream);
                    }

                    entidad.FotografiaFachada = "/fotos/" + nombreArchivo;
                }

                _context.DescripcionDelEdificios.Add(entidad);
                await _context.SaveChangesAsync();

                return RedirectToAction("Confirmacion", "Identificacion");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Registrar_Descripcion(int idIdentificacion)
        {
            var model = new DescripcionDelEdificioViewModel
            {
                IdIdentificacion = idIdentificacion
            };

            return View(model);
        }


    }
}

