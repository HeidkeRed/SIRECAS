using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    [Autorizado(1, 2)] // Solo Administrador y Editor
    public class DocumentoLegalController : Controller
    {
        private readonly SirecasContext _context;
        private readonly IWebHostEnvironment _env;

        public DocumentoLegalController(SirecasContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet]
        public IActionResult Registrar_DocumentoLegal(int idIdentificacion)
        {
            var model = new DocumentoLegalViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registrar_DocumentoLegal(DocumentoLegalViewModel model)
        {
            if (ModelState.IsValid)
            {
                var carpetaDocumentos = Path.Combine(_env.WebRootPath, "documentos_legales");
                if (!Directory.Exists(carpetaDocumentos))
                {
                    Directory.CreateDirectory(carpetaDocumentos);
                }

                var extension = Path.GetExtension(model.Archivo.FileName);
                var nombreArchivo = $"{Guid.NewGuid()}{extension}";
                var rutaCompleta = Path.Combine(carpetaDocumentos, nombreArchivo);

                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await model.Archivo.CopyToAsync(stream);
                }

                var documento = new DocumentoLegal
                {
                    IdIdentificacion = model.IdIdentificacion,
                    NombreDocumento = model.NombreDocumento,
                    RutaArchivo = "/documentos_legales/" + nombreArchivo
                };

                _context.DocumentoLegals.Add(documento);
                await _context.SaveChangesAsync();

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }
    }
}