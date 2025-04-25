using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class CubiertaViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        public bool Lamina { get; set; }

        public bool Concreto { get; set; }

        public bool Boveda { get; set; }

        public bool Cupula { get; set; }

        public bool Palma { get; set; }

        [Display(Name = "Losa plana")]
        public bool LosaPlana { get; set; }

        [Display(Name = "Dos aguas")]
        public bool DosAguas { get; set; }

        [Display(Name = "Tres o cuatro aguas")]
        public bool TresCuatroAguas { get; set; }

        [Display(Name = "Otras cubiertas")]
        public string? OtrasCubiertas { get; set; }

        public string? Observaciones { get; set; }
    }
}
