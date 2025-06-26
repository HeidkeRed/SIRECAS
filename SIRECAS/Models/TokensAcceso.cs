using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class TokensAcceso
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime Expira { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
