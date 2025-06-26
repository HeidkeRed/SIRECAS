using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Evita que el navegador guarde caché del Index (página pública de inicio)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario != null)
            {
                // Si ya está logeado, redirige al inicio privado
                return RedirectToAction("Inicio", "Home");
            }

            return View(); // Si no ha iniciado sesión, muestra Index normal
        }
        [Autorizado(1, 2, 3)]
        public IActionResult Inicio()
        {
            return View();
        }
    }
}