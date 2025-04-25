using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class RegistroTrabajo
{
    public int IdTrabajo { get; set; }

    public int IdIdentificacion { get; set; }

    public string? TipoRegistro { get; set; }

    public string? Descripcion { get; set; }

    public string? EmpresaResponsable { get; set; }

    public DateOnly? FechaTrabajo { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
