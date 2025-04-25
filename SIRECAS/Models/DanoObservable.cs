using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class DanoObservable
{
    public int IdDano { get; set; }

    public int IdIdentificacion { get; set; }

    public string Tipo { get; set; } = null!;

    public string Zona { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<DanoFoto> DanoFotos { get; set; } = new List<DanoFoto>();

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
