using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class ActividadRegistro
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Actividad { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public TimeOnly Hora { get; set; }
}
