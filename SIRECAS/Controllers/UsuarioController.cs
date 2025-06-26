using Microsoft.AspNetCore.Mvc;
using SIRECAS.Models;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace SIRECAS.Controllers
{
    [Autorizado(1)] // Solo admin
    public class UsuarioController : Controller
    {
        private readonly Sirecas2Context _context;

        private readonly EmailService _emailService;

        public UsuarioController(Sirecas2Context context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1)]
        public async Task<IActionResult> Registrar(RegistrarUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool existe = _context.Usuarios.Any(u => u.Email == model.Email);
            if (existe)
            {
                ModelState.AddModelError("Email", "El correo ya está registrado.");
                return View(model);
            }

            var hasher = new PasswordHasher<Usuario>();
            string hashedPassword = hasher.HashPassword(null, model.Contraseña);

            var nuevo = new Usuario
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Contraseña = hashedPassword,
                IdRol = model.IdRol,
                Autorizado = model.Autorizado,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(nuevo);
            await _context.SaveChangesAsync();

            // Enviar correo de bienvenida con indicaciones
            string asunto = "Bienvenido a SIRECAS";
            string cuerpoHtml = $@"
        <p>¡Hola {nuevo.Nombre}!</p>
        <p>Se ha creado una cuenta para ti en SIRECAS con el siguiente usuario:</p>
        <ul>
            <li><strong>Usuario (Email):</strong> {nuevo.Email}</li>
        </ul>
        <p>Por seguridad, tu contraseña no se envía por correo.</p>
        <p>Si olvidaste tu contraseña, utiliza la opción <strong>“¿Olvidó su contraseña?”</strong> en la página de inicio de sesión para restablecerla.</p>
        <p>Saludos,<br/>El equipo de SIRECAS</p>
    ";

            await _emailService.EnviarCorreoAsync(nuevo.Email, asunto, cuerpoHtml);

            TempData["Exito"] = "Usuario registrado exitosamente y correo de bienvenida enviado.";
            return RedirectToAction("Registrar");
        }



        public async Task<IActionResult> ListaUsuarios()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarAutorizacion(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Autorizado = !usuario.Autorizado;  // sin '?? false'
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ListaUsuarios");
        }



        [HttpPost]
        [Autorizado(1)]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ListaUsuarios");
        }

        [HttpPost]
        [Autorizado(1)]
        public async Task<IActionResult> Actualizar(Usuario usuarioActualizado)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioActualizado.IdUsuario);
            if (usuario != null)
            {
                usuario.Nombre = usuarioActualizado.Nombre;
                usuario.Email = usuarioActualizado.Email;
                usuario.IdRol = usuarioActualizado.IdRol;
                // No actualizamos contraseña aquí por seguridad, puede ir en otro flujo.
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ListaUsuarios");
        }
    }
}
