using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class PlanimetriaExistente
{
    public int IdPlanimetria { get; set; }

    public int IdIdentificacion { get; set; }

    public string TipoPlanimetria { get; set; } = null!;

    public string RutaArchivo { get; set; } = null!;

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
