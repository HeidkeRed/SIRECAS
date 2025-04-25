using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class AcabadosPlafoneViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        [Display(Name = "Aparente sin acabado final")]
        public bool AparenteSinAcabadoFinal { get; set; }

        [Display(Name = "Base cemento")]
        public bool BaseCemento { get; set; }

        [Display(Name = "Base yeso")]
        public bool BaseYeso { get; set; }

        public bool Pintura { get; set; }

        [Display(Name = "Otros acabados")]
        public string? OtrosAcabados { get; set; }

        public string? Observaciones { get; set; }
    }
}
