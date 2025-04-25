using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class CimientoViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        [Display(Name = "Tierra compactada")]
        public bool TierraCompactada { get; set; }

        [Display(Name = "Mampostería de piedra")]
        public bool MamposteriaPiedra { get; set; }

        [Display(Name = "Zapatas aisladas")]
        public bool ZapatasAisladas { get; set; }

        [Display(Name = "Zapatas corridas")]
        public bool ZapatasCorridas { get; set; }

        [Display(Name = "Losa de cimentación")]
        public bool LosaCimentacion { get; set; }

        [Display(Name = "Otros cimientos")]
        public string? OtrosCimientos { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }
}

