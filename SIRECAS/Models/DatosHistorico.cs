using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class DatosHistorico
{
    public int IdDatoHistorico { get; set; }

    public int IdIdentificacion { get; set; }

    public string? Titulo { get; set; }

    public string Descripcion { get; set; } = null!;

    public DateOnly? FechaEvento { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
