using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class VentanaViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        public bool Madera { get; set; }

        public bool Herreria { get; set; }

        public bool Aluminio { get; set; }

        [Display(Name = "Otros materiales")]
        public string? OtrosMateriales { get; set; }

        public string? Observaciones { get; set; }
    }
}
