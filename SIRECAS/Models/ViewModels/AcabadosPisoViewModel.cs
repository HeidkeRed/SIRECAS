using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class AcabadosPisoViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        public bool Tierra { get; set; }

        public bool Mosaico { get; set; }

        [Display(Name = "Piso cerámico")]
        public bool PisoCeramico { get; set; }

        [Display(Name = "Concreto pulido")]
        public bool ConcretoPulido { get; set; }

        [Display(Name = "Otros acabados")]
        public string? OtrosAcabados { get; set; }

        public string? Observaciones { get; set; }
    }
}
