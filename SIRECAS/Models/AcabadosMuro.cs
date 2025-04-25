using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class AcabadosMuro
{
    public int IdAcabadoMuro { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? Aparente { get; set; }

    public bool? BaseCemento { get; set; }

    public bool? Yeso { get; set; }

    public bool? Pintura { get; set; }

    public string? OtrosAcabados { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
