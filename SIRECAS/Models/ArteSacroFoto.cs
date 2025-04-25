using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class ArteSacroFoto
{
    public int IdFoto { get; set; }

    public int IdElemento { get; set; }

    public string RutaArchivo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ElementoArteSacro IdElementoNavigation { get; set; } = null!;
}
