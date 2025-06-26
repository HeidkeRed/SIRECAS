using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    [Autorizado(1, 2)] // Solo Administrador y Editor
    public class DocumentoLegalController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly IWebHostEnvironment _env;
        private readonly ActividadService _actividadService;

        public DocumentoLegalController(Sirecas2Context context, IWebHostEnvironment env, ActividadService actividadService)
        {
            _context = context;
            _env = env;
            _actividadService = actividadService;
        }

        [HttpGet]
        [Autorizado(1, 2)]
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
        [Autorizado(1, 2)]
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

                // Registrar actividad
                await _actividadService.RegistrarActividadAsync($"registró un documento legal", model.IdIdentificacion);

                return RedirectToAction("MenuSeccionesRegistro", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> ActualizarNombreDocumentoLegal(int idDocumento, string nuevoNombre)
        {
            var documento = await _context.DocumentoLegals.FindAsync(idDocumento);
            if (documento != null)
            {
                documento.NombreDocumento = nuevoNombre;
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync($"actualizó el nombre del documento legal", documento.IdIdentificacion);

                return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = documento.IdIdentificacion });
            }


            TempData["ErrorMessage"] = "Documento no encontrado.";
            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = 0 });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarDocumentoLegal(int idDocumento)
        {
            var documento = await _context.DocumentoLegals.FindAsync(idDocumento);
            if (documento != null)
            {
                var rutaArchivo = Path.Combine(_env.WebRootPath, documento.RutaArchivo.TrimStart('/'));
                if (System.IO.File.Exists(rutaArchivo))
                    System.IO.File.Delete(rutaArchivo);

                int idIdentificacion = documento.IdIdentificacion;

                // Remover documento y guardar cambios
                _context.DocumentoLegals.Remove(documento);
                await _context.SaveChangesAsync();

                await _actividadService.RegistrarActividadAsync("eliminó un documento legal", idIdentificacion);

                return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion });
            }

            TempData["ErrorMessage"] = "Documento no encontrado.";
            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = 0 });
        }

    }
}