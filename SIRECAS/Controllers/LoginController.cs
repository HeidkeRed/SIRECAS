using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Helpers;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;

namespace SIRECAS.Controllers
{
    public class LoginController : Controller
    {
        private readonly Sirecas2Context _context;
        private readonly EmailService _emailService;


        public LoginController(Sirecas2Context context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario != null)
            {
                return RedirectToAction("Inicio", "Home"); // O a donde quieras redirigir al usuario logeado
            }

            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario != null)
            {
                return RedirectToAction("Inicio", "Home");
            }

            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == model.Email && u.Autorizado == true);

            if (usuario == null)
            {
                var actividadFallida = new ActividadRegistro
                {
                    UsuarioId = 0,
                    Actividad = $"Intento fallido de inicio de sesión con el correo: {model.Email}",
                    Fecha = DateTime.Now.Date,
                    Hora = TimeOnly.FromDateTime(DateTime.Now)
                };
                _context.ActividadRegistro.Add(actividadFallida);
                _context.SaveChanges();

                ModelState.AddModelError(string.Empty, "Credenciales inválidas o no autorizado.");
                return View(model);
            }

            var hasher = new PasswordHasher<Usuario>();
            var resultado = hasher.VerifyHashedPassword(null, usuario.Contraseña, model.Contraseña);

            if (resultado == PasswordVerificationResult.Failed)
            {
                var actividadFallida = new ActividadRegistro
                {
                    UsuarioId = usuario.IdUsuario,
                    Actividad = $"Contraseña incorrecta para el usuario: {usuario.Nombre} ({usuario.Email})",
                    Fecha = DateTime.Now.Date,
                    Hora = TimeOnly.FromDateTime(DateTime.Now)
                };
                _context.ActividadRegistro.Add(actividadFallida);
                _context.SaveChanges();

                ModelState.AddModelError(string.Empty, "Contraseña incorrecta.");
                return View(model);
            }

            // Generar token de 6 dígitos
            var token = new Random().Next(100000, 999999).ToString();

            var tokenAcceso = new TokensAcceso
            {
                UsuarioId = usuario.IdUsuario,
                Token = token,
                Expira = DateTime.Now.AddMinutes(10)
            };
            _context.TokensAccesos.Add(tokenAcceso);
            _context.SaveChanges();

            // Enviar correo con token
            await _emailService.EnviarCorreoAsync(
                usuario.Email,
                "Código de verificación",
                $"Tu código de verificación es: {token}. Este código expirará en 10 minutos."
            );

            // Guardar el Id para verificar el token después
            HttpContext.Session.SetInt32("IdVerificacion", usuario.IdUsuario);

            // Redirigir a la acción que mostrará el formulario para ingresar el token
            return RedirectToAction("VerificarToken");
        }




        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home"); // O a donde quieras redirigir
        }

        [HttpGet]
        public IActionResult VerificarToken()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerificarToken(VerificarTokenViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var idUsuario = HttpContext.Session.GetInt32("IdVerificacion");

            if (idUsuario == null)
            {
                ModelState.AddModelError(string.Empty, "Sesión inválida, por favor inicie sesión nuevamente.");
                return View(model);
            }

            var tokenBD = _context.TokensAccesos
             .FirstOrDefault(t =>
                 t.UsuarioId == idUsuario &&
                 t.Token == model.Token &&
                 t.Expira > DateTime.Now);

            if (tokenBD == null)
            {
                ModelState.AddModelError(string.Empty, "Código inválido o expirado.");
                return View(model);
            }

            // Token válido: proceder a autenticar el usuario
            var usuario = _context.Usuarios.Find(idUsuario);

            if (usuario == null)
            {
                ModelState.AddModelError(string.Empty, "Usuario no encontrado.");
                return View(model);
            }

            // Guardar sesión definitiva
            HttpContext.Session.SetInt32("IdUsuario", usuario.IdUsuario);
            HttpContext.Session.SetString("NombreUsuario", usuario.Nombre);
            HttpContext.Session.SetInt32("IdRol", usuario.IdRol ?? 0);

            // Guardar actividad de inicio exitoso
            var actividadExitosa = new ActividadRegistro
            {
                UsuarioId = usuario.IdUsuario,
                Actividad = $"Inicio de sesión exitoso de: {usuario.Nombre}",
                Fecha = DateTime.Now.Date,
                Hora = TimeOnly.FromDateTime(DateTime.Now)
            };
            _context.ActividadRegistro.Add(actividadExitosa);
            _context.SaveChanges();

            // Limpiar la sesión temporal de verificación
            HttpContext.Session.Remove("IdVerificacion");

            // Redirigir a la página principal o dashboard
            return RedirectToAction("Inicio", "Home");
        }

        [HttpGet]
        public IActionResult RecuperarContrasena()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecuperarContrasena(string email)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (usuario == null)
            {
                ModelState.AddModelError("", "El correo no está registrado.");
                return View();
            }

            // Generar token
            var token = Guid.NewGuid().ToString();
            var tokenAcceso = new TokensAcceso
            {
                UsuarioId = usuario.IdUsuario,
                Token = token,
                Expira = DateTime.Now.AddHours(1)
            };
            _context.TokensAccesos.Add(tokenAcceso);
            await _context.SaveChangesAsync();

            // Crear enlace
            var enlace = Url.Action("RestablecerContrasena", "Login",
                new { email = usuario.Email, token = token }, Request.Scheme);

            var mensaje = $@"
            <p>Hola {usuario.Nombre},</p>
            <p>Hemos recibido una solicitud para restablecer tu contraseña. Haz clic en el siguiente enlace:</p>
            <p><a href='{enlace}'>Restablecer contraseña</a></p>
            <p>Si no solicitaste esto, puedes ignorar este correo.</p>";

            await _emailService.EnviarCorreoAsync(usuario.Email, "Restablecer contraseña", mensaje);

            TempData["Exito"] = "Se ha enviado un correo para restablecer tu contraseña.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> RestablecerContrasena(string email, string token)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.TokensAccesos)
                .FirstOrDefaultAsync(u => u.Email == email);

            if (usuario == null || !usuario.TokensAccesos.Any(t => t.Token == token && t.Expira > DateTime.Now))
            {
                return BadRequest("Token inválido o expirado.");
            }

            var model = new RestablecerContrasenaViewModel { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestablecerContrasena(RestablecerContrasenaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Validación adicional por seguridad
            var passwordRegex = new System.Text.RegularExpressions.Regex(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{10,}$");
            if (!passwordRegex.IsMatch(model.NuevaContrasena))
            {
                ModelState.AddModelError("NuevaContrasena", "La contraseña debe tener al menos una mayúscula, un número y un símbolo, y un mínimo de 10 caracteres.");
                return View(model);
            }

            var usuario = await _context.Usuarios
                .Include(u => u.TokensAccesos)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (usuario == null)
            {
                return BadRequest("Usuario no encontrado.");
            }

            var tokenValido = usuario.TokensAccesos
                .FirstOrDefault(t => t.Token == model.Token && t.Expira > DateTime.Now);

            if (tokenValido == null)
            {
                return BadRequest("Token inválido o expirado.");
            }

            var hasher = new PasswordHasher<Usuario>();
            usuario.Contraseña = hasher.HashPassword(usuario, model.NuevaContrasena);

            // Eliminar el token usado
            _context.TokensAccesos.Remove(tokenValido);

            await _context.SaveChangesAsync();

            TempData["Exito"] = "Tu contraseña ha sido restablecida correctamente.";
            return RedirectToAction("Login");
        }


    }
}