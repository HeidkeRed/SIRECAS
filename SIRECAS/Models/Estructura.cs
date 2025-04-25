using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Estructura
{
    public int IdEstructura { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? MuroPiedra { get; set; }

    public bool? MuroBlock { get; set; }

    public bool? MuroLadrillo { get; set; }

    public bool? MuroAdobe { get; set; }

    public bool? MuroBahareque { get; set; }

    public string? OtroTipoMuro { get; set; }

    public bool? ColumnaConcreto { get; set; }

    public bool? ColumnaPiedra { get; set; }

    public string? OtroTipoColumna { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
