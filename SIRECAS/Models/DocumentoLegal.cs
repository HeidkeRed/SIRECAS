using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class DocumentoLegal
{
    public int Id { get; set; }

    public int IdIdentificacion { get; set; }

    public string NombreDocumento { get; set; } = null!;

    public string RutaArchivo { get; set; } = null!;

    public DateTime FechaSubida { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
