using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO;

namespace SIRECAS.Controllers
{
    public class FotosController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly IWebHostEnvironment _env;

        public FotosController(Sirecas2Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: SubirFotos
        public IActionResult SubirFotos(int idIdentificacion)
        {
            var model = new FotoSubidaViewModel
            {
                IdIdentificacion = idIdentificacion
            };
            return View(model);
        }

        // POST: SubirFotos
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> SubirFotos(FotoSubidaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Archivo == null || model.Archivo.Length == 0)
            {
                ModelState.AddModelError("", "No se ha seleccionado ningún archivo.");
                return View(model);
            }

            var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(model.Archivo.FileName);
            var rutaCarpeta = Path.Combine(_env.WebRootPath, "fotos");
            var rutaArchivo = Path.Combine(rutaCarpeta, nombreArchivo);

            Directory.CreateDirectory(rutaCarpeta);

            try
            {
                using (var image = await Image.LoadAsync(model.Archivo.OpenReadStream()))
                {
                    // Redimensionar si es muy grande (opcional)
                    if (image.Width > 1920 || image.Height > 1080)
                    {
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Mode = ResizeMode.Max,
                            Size = new Size(1920, 1080)
                        }));
                    }

                    // Ajustar calidad y comprimir
                    var encoder = new JpegEncoder
                    {
                        Quality = 75
                    };

                    using (var ms = new MemoryStream())
                    {
                        await image.SaveAsJpegAsync(ms, encoder);

                        if (ms.Length > 2 * 1024 * 1024)
                        {
                            ModelState.AddModelError("", "La imagen comprimida sigue siendo demasiado grande (mayor a 2 MB). Por favor sube una imagen más pequeña.");
                            return View(model);
                        }

                        ms.Position = 0;
                        using (var fs = new FileStream(rutaArchivo, FileMode.Create))
                        {
                            await ms.CopyToAsync(fs);
                        }
                    }
                }

                // Guardar referencia en la base de datos
                var nuevaFoto = new Foto
                {
                    IdIdentificacion = model.IdIdentificacion,
                    NombreArchivo = nombreArchivo,
                    Descripcion = model.Descripcion,
                    FechaSubida = DateTime.Now
                };

                _context.Fotos.Add(nuevaFoto);
                await _context.SaveChangesAsync();

                return RedirectToAction("SubirFotos", new { idIdentificacion = model.IdIdentificacion });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al procesar la imagen: " + ex.Message);
                return View(model);
            }
        }

        // GET: Eliminar
        public async Task<IActionResult> Eliminar(int id)
        {
            var foto = await _context.Fotos.FindAsync(id);
            if (foto == null)
            {
                return NotFound();
            }

            return View(foto);
        }

        // POST: EliminarConfirmado
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var foto = await _context.Fotos.FindAsync(id);
            if (foto == null)
            {
                return NotFound();
            }

            var rutaArchivo = Path.Combine(_env.WebRootPath, "fotos", foto.NombreArchivo);
            if (System.IO.File.Exists(rutaArchivo))
            {
                System.IO.File.Delete(rutaArchivo);
            }

            _context.Fotos.Remove(foto);
            await _context.SaveChangesAsync();

            return RedirectToAction("Resumen", "Identificacion", new { IdIdentificacion = foto.IdIdentificacion });
        }
    }
}
