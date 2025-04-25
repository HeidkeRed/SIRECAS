using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Ventana
{
    public int IdVentana { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? Madera { get; set; }

    public bool? Herreria { get; set; }

    public bool? Aluminio { get; set; }

    public string? OtrosMateriales { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
