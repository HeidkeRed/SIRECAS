using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class ElementoArteSacro
{
    public int IdElemento { get; set; }

    public int IdIdentificacion { get; set; }

    public string? Seccion { get; set; }

    public string? Subtipo { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<ArteSacroFoto> ArteSacroFotos { get; set; } = new List<ArteSacroFoto>();

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
