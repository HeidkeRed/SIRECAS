using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class AcabadosPlafone
{
    public int IdAcabadoPlafon { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? AparenteSinAcabadoFinal { get; set; }

    public bool? BaseCemento { get; set; }

    public bool? BaseYeso { get; set; }

    public bool? Pintura { get; set; }

    public string? OtrosAcabados { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
