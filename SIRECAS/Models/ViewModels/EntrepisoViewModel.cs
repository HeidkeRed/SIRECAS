using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class EntrepisoViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        [Display(Name = "Losa maciza")]
        public bool LosaMaciza { get; set; }

        [Display(Name = "Losa reticular")]
        public bool LosaReticular { get; set; }

        [Display(Name = "Vigueta y bovedilla")]
        public bool ViguetaBovedilla { get; set; }

        [Display(Name = "Otros entrepisos")]
        public string? OtrosEntrepisos { get; set; }

        [Display(Name = "No aplica")]
        public bool NoAplica { get; set; }

        public string? Observaciones { get; set; }
    }
}
