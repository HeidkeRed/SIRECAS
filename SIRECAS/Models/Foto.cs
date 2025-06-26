using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Foto
{
    public int Id { get; set; }

    public int IdIdentificacion { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public DateTime? FechaSubida { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
