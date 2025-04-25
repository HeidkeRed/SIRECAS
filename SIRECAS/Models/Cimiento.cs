using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Cimiento
{
    public int IdCimientos { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? TierraCompactada { get; set; }

    public bool? MamposteriaPiedra { get; set; }

    public bool? ZapatasAisladas { get; set; }

    public bool? ZapatasCorridas { get; set; }

    public bool? LosaCimentacion { get; set; }

    public string? OtrosCimientos { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
