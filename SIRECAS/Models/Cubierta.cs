using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Cubierta
{
    public int IdCubiertas { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? Lamina { get; set; }

    public bool? Concreto { get; set; }

    public bool? Boveda { get; set; }

    public bool? Cupula { get; set; }

    public bool? Palma { get; set; }

    public bool? LosaPlana { get; set; }

    public bool? DosAguas { get; set; }

    public bool? TresCuatroAguas { get; set; }

    public string? OtrasCubiertas { get; set; }

    public string? Observaciones { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
