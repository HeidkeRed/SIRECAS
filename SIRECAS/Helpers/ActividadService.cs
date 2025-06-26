using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;      // Cambia esto por el namespace de tu DbContext
using SIRECAS.Models;     // Cambia esto por donde están tus modelos

namespace SIRECAS.Helpers  // Este es el namespace completo si está en Helpers
{
    public class ActividadService
    {
        private readonly Sirecas2Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ActividadService(Sirecas2Context context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task RegistrarActividadAsync(string mensaje, int idIdentificacion)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var usuarioId = httpContext?.Session.GetInt32("IdUsuario");
            var nombreUsuario = httpContext?.Session.GetString("NombreUsuario") ?? "Usuario desconocido";

            var identificacion = await _context.Identificacions
                .FirstOrDefaultAsync(x => x.IdIdentificacion == idIdentificacion);
            var nombreParroquia = identificacion?.NombreParroquia ?? "Parroquia desconocida";

            if (usuarioId.HasValue)
            {
                var actividad = new ActividadRegistro
                {
                    UsuarioId = usuarioId.Value,
                    Actividad = $"{nombreUsuario} {mensaje} en la parroquia '{nombreParroquia}'.",
                    Fecha = DateTime.Now.Date,
                    Hora = TimeOnly.FromDateTime(DateTime.Now)
                };

                _context.ActividadRegistro.Add(actividad);
                await _context.SaveChangesAsync();
            }
        }
    }
}


