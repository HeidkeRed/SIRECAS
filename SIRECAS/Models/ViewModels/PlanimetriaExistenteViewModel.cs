using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class PlanimetriaExistenteViewModel
    {
        public int IdIdentificacion { get; set; }

        [Required]
        public string TipoPlanimetria { get; set; } = string.Empty;

        [Required]
        public IFormFile Archivo { get; set; } = null!;

        public string? Observaciones { get; set; }
    }
}
