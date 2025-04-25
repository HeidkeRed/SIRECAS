using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class DanoFoto
{
    public int IdFoto { get; set; }

    public int IdDano { get; set; }

    public string RutaArchivo { get; set; } = null!;

    public string? Observacion { get; set; }

    public virtual DanoObservable IdDanoNavigation { get; set; } = null!;
}
