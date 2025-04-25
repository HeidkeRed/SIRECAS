using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class LoginController : Controller
    {
        private readonly SirecasContext _context;

        public LoginController(SirecasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Buscar al usuario solo por correo
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == model.Email && u.Autorizado == true);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas o no autorizado.");
                return View(model);
            }

            // Verificar contraseña hasheada
            var hasher = new PasswordHasher<Usuario>();
            var resultado = hasher.VerifyHashedPassword(null, usuario.Contraseña, model.Contraseña);

            if (resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Contraseña incorrecta.");
                return View(model);
            }

            // Crear sesión si todo es correcto
            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
            HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            HttpContext.Session.SetInt32("IdRol", usuario.IdRol ?? 0);

            return RedirectToAction("Inicio", "Home");
        }


        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); // O a donde quieras redirigir
        }
    }
}

