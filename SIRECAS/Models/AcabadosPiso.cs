using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class AcabadosPiso
{
    public int IdAcabadoPiso { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? Tierra { get; set; }

    public bool? Mosaico { get; set; }

    public bool? PisoCeramico { get; set; }

    public bool? ConcretoPulido { get; set; }

    public string? OtrosAcabados { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
