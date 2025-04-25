using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class AcabadosMuroViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        public bool Aparente { get; set; }

        [Display(Name = "Base cemento")]
        public bool BaseCemento { get; set; }

        public bool Yeso { get; set; }

        public bool Pintura { get; set; }

        [Display(Name = "Otros acabados")]
        public string? OtrosAcabados { get; set; }

        public string? Observaciones { get; set; }
    }
}
