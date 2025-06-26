using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int? IdRol { get; set; }

    public bool? Autorizado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual Role? IdRolNavigation { get; set; }

    public virtual ICollection<TokensAcceso> TokensAccesos { get; set; } = new List<TokensAcceso>();
}
