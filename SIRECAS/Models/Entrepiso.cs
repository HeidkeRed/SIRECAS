using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Entrepiso
{
    public int IdEntrepisos { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? LosaMaciza { get; set; }

    public bool? LosaReticular { get; set; }

    public bool? ViguetaBovedilla { get; set; }

    public string? OtrosEntrepisos { get; set; }

    public bool? NoAplica { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
