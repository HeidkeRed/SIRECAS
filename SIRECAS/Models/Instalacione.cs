using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Instalacione
{
    public int IdInstalacion { get; set; }

    public int IdIdentificacion { get; set; }

    public bool? ElectricaVisible { get; set; }

    public bool? ElectricaOculta { get; set; }

    public string? ObservacionesElectrica { get; set; }

    public bool? SanitariaVisible { get; set; }

    public bool? SanitariaOculta { get; set; }

    public string? ObservacionesSanitaria { get; set; }

    public bool? HidraulicaVisible { get; set; }

    public bool? HidraulicaOculta { get; set; }

    public string? ObservacionesHidraulica { get; set; }

    public bool? GasVisible { get; set; }

    public bool? GasOculta { get; set; }

    public string? ObservacionesGas { get; set; }

    public bool? TelefoniaVisible { get; set; }

    public bool? TelefoniaOculta { get; set; }

    public string? ObservacionesTelefonia { get; set; }

    public bool? OtroVisible { get; set; }

    public bool? OtroOculta { get; set; }

    public string? ObservacionesOtro { get; set; }

    public virtual Identificacion IdIdentificacionNavigation { get; set; } = null!;
}
