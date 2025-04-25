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
        private readonly SirecasContext _context;

        public UsuarioController(SirecasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrar(RegistrarUsuarioViewModel model)
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
            _context.SaveChanges();

            TempData["Exito"] = "Usuario registrado exitosamente.";
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
                usuario.Autorizado = !(usuario.Autorizado ?? false);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("ListaUsuarios");
        }


        [HttpPost]
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
