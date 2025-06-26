using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Instalacione
{
    public int Id { get; set; }

    public int IdIdentificacion { get; set; }

    public string Tipo { get; set; } = null!;

    public bool Visible { get; set; }

    public bool Oculta { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
