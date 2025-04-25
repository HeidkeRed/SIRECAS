using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class DescripcionDelEdificio
{
    public int IdDescripcion { get; set; }

    public int IdIdentificacion { get; set; }

    public string? DescripcionGeneral { get; set; }

    public string? DescripcionFachada { get; set; }

    public string? FotografiaFachada { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
