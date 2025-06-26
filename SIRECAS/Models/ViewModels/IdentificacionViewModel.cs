using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class IdentificacionViewModel
    {
        [Required]
        [Display(Name = "Nombre de la Parroquia")]
        public string NombreParroquia { get; set; }

        [Required]
        [Display(Name = "Nombre del Titular")]
        public string NombreTitular { get; set; }

        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "Año de Construcción")]
        public int? AnioConstruccion { get; set; }

        public string Calle { get; set; }
        public string NumeroEdificio { get; set; }
        public string EntreCalles { get; set; }
        public string Colonia { get; set; }

        [Display(Name = "Código Postal")]
        public string CodigoPostal { get; set; }

        public string Municipio { get; set; }

        [Display(Name = "Ubicación en Manzana")]
        public string UbicacionManzana { get; set; }

        public string ConstruccionesColindantes { get; set; }

        public decimal? Frente { get; set; }
        public decimal? Fondo { get; set; }

        [Display(Name = "M² de Terreno")]
        public decimal? M2Terreno { get; set; }

        [Display(Name = "M² de Construcción")]
        public decimal? M2Construccion { get; set; }

        [Display(Name = "Coordenada X (Longitud)")]
        [RegularExpression(@"^-?\d{1,3}(\.\d{1,10})?$",
            ErrorMessage = "La coordenada X debe tener el formato decimal correcto (ej. 19.123456).")]
        public string CoordenadaX { get; set; }

        [Display(Name = "Coordenada Y (Latitud)")]
        [RegularExpression(@"^-?\d{1,3}(\.\d{1,10})?$",
            ErrorMessage = "La coordenada Y debe tener el formato decimal correcto (ej. -98.123456).")]
        public string CoordenadaY { get; set; }
    }
}

