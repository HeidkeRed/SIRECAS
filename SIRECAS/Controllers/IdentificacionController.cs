using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class IdentificacionController : Controller
    {
        private readonly SirecasContext _context;

        public IdentificacionController(SirecasContext context)
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
                    CoordenadaX = model.CoordenadaX,
                    CoordenadaY = model.CoordenadaY
                };

                _context.Identificacions.Add(entidad);
                await _context.SaveChangesAsync();

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
        public IActionResult MenuSeccionesRegistro(int idIdentificacion)
        {
            ViewBag.IdIdentificacion = idIdentificacion;
            return View();
        }

        [HttpGet]
        [Autorizado(1, 2, 3)] // Todos pueden ver el resumen
        public async Task<IActionResult> Resumen(int idIdentificacion)
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
                .FirstOrDefaultAsync(i => i.IdIdentificacion == idIdentificacion);

            if (identificacion == null)
                return NotFound();

            // Aquí detectamos la IP
            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var ipAutorizadas = new List<string>
    {
        "127.0.0.1",
        "::1",
        "192.168.1.22"
    };

            // Si la IP no está autorizada, ocultamos los documentos legales
            if (!ipAutorizadas.Contains(userIp))
            {
                identificacion.DocumentoLegals = new List<DocumentoLegal>();
            }

            return View(identificacion);
        }
    }
}
