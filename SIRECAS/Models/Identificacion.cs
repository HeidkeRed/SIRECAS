using System;
using System.Collections.Generic;

namespace SIRECAS.Models;

public partial class Identificacion
{
    public int IdIdentificacion { get; set; }

    public string NombreParroquia { get; set; } = null!;

    public string NombreTitular { get; set; } = null!;

    public string? CorreoElectronico { get; set; }

    public string? Telefono { get; set; }

    public int? AnioConstruccion { get; set; }

    public string? Calle { get; set; }

    public string? NumeroEdificio { get; set; }

    public string? EntreCalles { get; set; }

    public string? Colonia { get; set; }

    public string? CodigoPostal { get; set; }

    public string? Municipio { get; set; }

    public string? UbicacionManzana { get; set; }

    public string? ConstruccionesColindantes { get; set; }

    public decimal? Frente { get; set; }

    public decimal? Fondo { get; set; }

    public decimal? M2terreno { get; set; }

    public decimal? M2construccion { get; set; }

    public decimal? CoordenadaX { get; set; }

    public decimal? CoordenadaY { get; set; }

    public virtual AcabadosMuro? AcabadosMuro { get; set; }

    public virtual AcabadosPiso? AcabadosPiso { get; set; }

    public virtual AcabadosPlafone? AcabadosPlafone { get; set; }

    public virtual Cimiento? Cimiento { get; set; }

    public virtual Cubierta? Cubierta { get; set; }

    public virtual ICollection<DanoObservable> DanoObservables { get; set; } = new List<DanoObservable>();

    public virtual ICollection<DatosHistorico> DatosHistoricos { get; set; } = new List<DatosHistorico>();

    public virtual ICollection<DescripcionDelEdificio> DescripcionDelEdificios { get; set; } = new List<DescripcionDelEdificio>();

    public virtual ICollection<DocumentoLegal> DocumentoLegals { get; set; } = new List<DocumentoLegal>();

    public virtual ICollection<ElementoArteSacro> ElementoArteSacros { get; set; } = new List<ElementoArteSacro>();

    public virtual Entrepiso? Entrepiso { get; set; }

    public virtual Estructura? Estructura { get; set; }

    public virtual ICollection<Foto> Fotos { get; set; } = new List<Foto>();

    public virtual ICollection<PlanimetriaExistente> PlanimetriaExistentes { get; set; } = new List<PlanimetriaExistente>();

    public virtual Puerta? Puerta { get; set; }

    public virtual ICollection<RegistroTrabajo> RegistroTrabajos { get; set; } = new List<RegistroTrabajo>();

    public virtual ICollection<Instalacione> Instalacione { get; set; } = new List<Instalacione>();

    public virtual Ventana? Ventana { get; set; }
}
