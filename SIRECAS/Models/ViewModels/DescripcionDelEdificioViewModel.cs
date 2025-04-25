using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SIRECAS.Models.ViewModels
{
    public class DescripcionDelEdificioViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        [Display(Name = "Descripción general")]
        public string? DescripcionGeneral { get; set; }

        [Display(Name = "Descripción de la fachada")]
        public string? DescripcionFachada { get; set; }

        [Display(Name = "Fotografía de la fachada")]
        public IFormFile? Foto { get; set; } // ← Para el archivo subido
    }
}
