using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class IdentificacionController : Controller
    {
        private readonly Sirecas2Context _context;

        public IdentificacionController(Sirecas2Context context)
        {
            _context = context;
        }


        [HttpGet]
        [Autorizado(1, 2)] // Solo Admin y Editor
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)] // Solo Admin y Editor
        public async Task<IActionResult> Index(IdentificacionViewModel model)
        {
            if (ModelState.IsValid)
            {
                decimal? coordenadaX = null;
                if (!string.IsNullOrWhiteSpace(model.CoordenadaX) && decimal.TryParse(model.CoordenadaX, out var x))
                {
                    coordenadaX = x;
                }

                decimal? coordenadaY = null;
                if (!string.IsNullOrWhiteSpace(model.CoordenadaY) && decimal.TryParse(model.CoordenadaY, out var y))
                {
                    coordenadaY = y;
                }

                var entidad = new Identificacion
                {
                    NombreParroquia = model.NombreParroquia,
                    NombreTitular = model.NombreTitular,
                    CorreoElectronico = model.CorreoElectronico,
                    Telefono = model.Telefono,
                    AnioConstruccion = model.AnioConstruccion,
                    Calle = model.Calle,
                    NumeroEdificio = model.NumeroEdificio,
                    EntreCalles = model.EntreCalles,
                    Colonia = model.Colonia,
                    CodigoPostal = model.CodigoPostal,
                    Municipio = model.Municipio,
                    UbicacionManzana = model.UbicacionManzana,
                    ConstruccionesColindantes = model.ConstruccionesColindantes,
                    Frente = model.Frente,
                    Fondo = model.Fondo,
                    M2terreno = model.M2Terreno,
                    M2construccion = model.M2Construccion,
                    CoordenadaX = coordenadaX,
                    CoordenadaY = coordenadaY
                };

                _context.Identificacions.Add(entidad);
                await _context.SaveChangesAsync();

                // Registro de actividad
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
                if (idUsuario != null)
                {
                    var actividad = new ActividadRegistro
                    {
                        UsuarioId = idUsuario.Value,
                        Actividad = $"Registró una nueva ficha de Identificación: {entidad.NombreParroquia}.",
                        Fecha = DateTime.Now.Date,
                        Hora = TimeOnly.FromDateTime(DateTime.Now)
                    };

                    _context.ActividadRegistro.Add(actividad);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Confirmacion");
            }

            return View(model);
        }



        public IActionResult Confirmacion()
        {
            return View(); // Esta puede quedar abierta si solo es mensaje
        }

        [Autorizado(1, 2, 3)] // Todos
        public async Task<IActionResult> Lista_identificaciones()
        {
            var registros = await _context.Identificacions.ToListAsync();
            return View(registros);
        }

        [Autorizado(1, 2)] // Solo Admin y Editor
        public IActionResult MenuSeccionesRegistro(int IdIdentificacion)
        {
            ViewBag.IdIdentificacion = IdIdentificacion;
            return View();
        }

        [HttpGet]
        [Autorizado(1, 2, 3)] // Todos pueden ver el resumen
        public async Task<IActionResult> Resumen(int IdIdentificacion)
        {
            var identificacion = await _context.Identificacions
                .Include(i => i.DescripcionDelEdificios)
                .Include(i => i.Cimiento)
                .Include(i => i.Estructura)
                .Include(i => i.Entrepiso)
                .Include(i => i.Cubierta)
                .Include(i => i.AcabadosPiso)
                .Include(i => i.AcabadosMuro)
                .Include(i => i.AcabadosPlafone)
                .Include(i => i.Puerta)
                .Include(i => i.Ventana)
                .Include(i => i.Instalacione)
                .Include(i => i.DatosHistoricos)
                .Include(i => i.ElementoArteSacros)
                    .ThenInclude(e => e.ArteSacroFotos)
                .Include(i => i.DanoObservables)
                    .ThenInclude(d => d.DanoFotos)
                .Include(i => i.RegistroTrabajos)
                .Include(i => i.PlanimetriaExistentes)
                .Include(i => i.DocumentoLegals)
                .Include(i => i.Fotos)
                .FirstOrDefaultAsync(i => i.IdIdentificacion == IdIdentificacion);

            if (identificacion == null)
                return NotFound();

            // 👤 Registrar actividad de consulta
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario != null)
            {
                var actividad = new ActividadRegistro
                {
                    UsuarioId = idUsuario.Value,
                    Actividad = $"Consultó la ficha de resumen de la parroquia: {identificacion.NombreParroquia}.",
                    Fecha = DateTime.Now.Date,
                    Hora = TimeOnly.FromDateTime(DateTime.Now)
                };

                _context.ActividadRegistro.Add(actividad);
                await _context.SaveChangesAsync();
            }

            // 🌐 Detectar IP y ocultar documentos si no está en la lista permitida
            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var ipAutorizadas = new List<string>
    {
        "127.0.0.1",
        "::1",
        "192.168.1.22"
    };

            if (!ipAutorizadas.Contains(userIp))
            {
                identificacion.DocumentoLegals = new List<DocumentoLegal>();
            }

            return View(identificacion);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)] // Que solo administradores y editores puedan actualizar
        public async Task<IActionResult> ActualizarIdentificacion(Identificacion model)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, regresa a la misma vista
                return View("Resumen", model);
            }

            var identificacion = await _context.Identificacions.FindAsync(model.IdIdentificacion);

            if (identificacion == null)
            {
                return NotFound();
            }

            // Actualizar los campos permitidos
            identificacion.NombreParroquia = model.NombreParroquia;
            identificacion.NombreTitular = model.NombreTitular;
            identificacion.CorreoElectronico = model.CorreoElectronico;
            identificacion.Telefono = model.Telefono;
            identificacion.AnioConstruccion = model.AnioConstruccion;
            identificacion.Calle = model.Calle;
            identificacion.NumeroEdificio = model.NumeroEdificio;
            identificacion.Colonia = model.Colonia;
            identificacion.CodigoPostal = model.CodigoPostal;
            identificacion.EntreCalles = model.EntreCalles;
            identificacion.Municipio = model.Municipio;
            identificacion.UbicacionManzana = model.UbicacionManzana;
            identificacion.ConstruccionesColindantes = model.ConstruccionesColindantes;
            identificacion.Frente = model.Frente;
            identificacion.Fondo = model.Fondo;
            identificacion.M2terreno = model.M2terreno;
            identificacion.M2construccion = model.M2construccion;
            identificacion.CoordenadaY = model.CoordenadaY;
            identificacion.CoordenadaX = model.CoordenadaX;

            // Guardar cambios
            await _context.SaveChangesAsync();

            // 👤 Registrar actividad
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario != null)
            {
                var actividad = new ActividadRegistro
                {
                    UsuarioId = idUsuario.Value,
                    Actividad = $"Actualizó la identificación de la parroquia: {model.NombreParroquia}.",
                    Fecha = DateTime.Now.Date,
                    Hora = TimeOnly.FromDateTime(DateTime.Now)
                };

                _context.ActividadRegistro.Add(actividad);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Información actualizada correctamente.";
            return RedirectToAction("Resumen", "Identificacion", new { idIdentificacion = model.IdIdentificacion });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1)] // Solo Admin
        public async Task<IActionResult> EliminarIdentificacion(int idIdentificacion)
        {
            var identificacion = await _context.Identificacions
                .Include(i => i.DescripcionDelEdificios)
                .Include(i => i.DanoObservables)
                .Include(i => i.Instalacione)
                .Include(i => i.DatosHistoricos)
                .Include(i => i.DocumentoLegals)
                .Include(i => i.ElementoArteSacros)
                .Include(i => i.PlanimetriaExistentes)
                .Include(i => i.RegistroTrabajos)
                .FirstOrDefaultAsync(i => i.IdIdentificacion == idIdentificacion);

            if (identificacion == null)
                return NotFound();

            // Verificar si hay elementos relacionados
            if (identificacion.DescripcionDelEdificios.Any() ||
                identificacion.DanoObservables.Any() ||
                identificacion.Instalacione.Any() ||
                identificacion.DatosHistoricos.Any() ||
                identificacion.DocumentoLegals.Any() ||
                identificacion.ElementoArteSacros.Any() ||
                identificacion.PlanimetriaExistentes.Any() ||
                identificacion.RegistroTrabajos.Any())
            {
                TempData["ErrorMessage"] = "No se puede eliminar. Existen datos relacionados con esta identificación.";
                return RedirectToAction("Resumen", new { idIdentificacion });
            }

            string nombreParroquia = identificacion.NombreParroquia;

            _context.Identificacions.Remove(identificacion);
            await _context.SaveChangesAsync();

            // Registrar actividad
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario != null)
            {
                var actividad = new ActividadRegistro
                {
                    UsuarioId = idUsuario.Value,
                    Actividad = $"Eliminó la identificación de la parroquia: {nombreParroquia}.",
                    Fecha = DateTime.Now.Date,
                    Hora = TimeOnly.FromDateTime(DateTime.Now)
                };

                _context.ActividadRegistro.Add(actividad);
                await _context.SaveChangesAsync();
            }

            TempData["SuccessMessage"] = "Identificación eliminada correctamente.";
            return RedirectToAction("Lista_identificaciones");
        }

    }
}
