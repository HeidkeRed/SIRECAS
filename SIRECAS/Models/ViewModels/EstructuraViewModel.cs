using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class EstructuraViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        [Display(Name = "Muro de piedra")]
        public bool MuroPiedra { get; set; }

        [Display(Name = "Muro de block")]
        public bool MuroBlock { get; set; }

        [Display(Name = "Muro de ladrillo")]
        public bool MuroLadrillo { get; set; }

        [Display(Name = "Muro de adobe")]
        public bool MuroAdobe { get; set; }

        [Display(Name = "Muro de bahareque")]
        public bool MuroBahareque { get; set; }

        [Display(Name = "Otro tipo de muro")]
        public string? OtroTipoMuro { get; set; }

        [Display(Name = "Columna de concreto")]
        public bool ColumnaConcreto { get; set; }

        [Display(Name = "Columna de piedra")]
        public bool ColumnaPiedra { get; set; }

        [Display(Name = "Otro tipo de columna")]
        public string? OtroTipoColumna { get; set; }

        public string? Observaciones { get; set; }
    }
}
