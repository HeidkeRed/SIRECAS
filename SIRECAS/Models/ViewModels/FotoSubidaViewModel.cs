using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace SIRECAS.Models.ViewModels
{
    public class FotoSubidaViewModel
    {
        public int IdIdentificacion { get; set; }

        [Required]
        [Display(Name = "Foto")]
        public IFormFile Archivo { get; set; } = null!;

        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }
    }
}
