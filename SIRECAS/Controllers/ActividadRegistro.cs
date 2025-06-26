using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SIRECAS.Models;
using System.Threading.Tasks;
using System.Linq;
using SIRECAS.Models.ViewModels;
using SIRECAS.Helpers;

namespace SIRECAS.Controllers
{
    public class ActividadRegistroController : Controller
    {
        private readonly Sirecas2Context _context;

        public ActividadRegistroController(Sirecas2Context context)
        {
            _context = context;
        }
        [Autorizado(1, 2)]
        public async Task<IActionResult> AdministradorActividad(int page = 1)
        {
            int pageSize = 20;

            var actividadesQuery = _context.ActividadRegistro
                .Join(_context.Usuarios,
                      actividad => actividad.UsuarioId,
                      usuario => usuario.IdUsuario,
                      (actividad, usuario) => new ActividadRegistroViewModel
                      {
                          Id = actividad.Id,
                          UsuarioId = usuario.IdUsuario,
                          NombreUsuario = usuario.Nombre,
                          Actividad = actividad.Actividad,
                          Fecha = actividad.Fecha,
                          Hora = actividad.Hora
                      })
                .OrderByDescending(a => a.Fecha)
                .ThenByDescending(a => a.Hora);

            // Traer todo a memoria primero (ojo con tablas muy grandes)
            var actividadesLista = await actividadesQuery.ToListAsync();

            int totalActividades = actividadesLista.Count;

            var actividadesPagina = actividadesLista
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new ActividadesPaginadasViewModel
            {
                Actividades = actividadesPagina,
                PaginaActual = page,
                TotalPaginas = (int)Math.Ceiling(totalActividades / (double)pageSize)
            };

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Autorizado(1, 2)]
        public async Task<IActionResult> EliminarTodas()
        {
            var todas = await _context.ActividadRegistro.ToListAsync();
            _context.ActividadRegistro.RemoveRange(todas);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Todas las actividades han sido eliminadas.";
            return RedirectToAction("AdministradorActividad");
        }



    }
}
