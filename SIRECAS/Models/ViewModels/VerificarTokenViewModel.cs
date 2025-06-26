using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class VerificarTokenViewModel
    {
        [Required(ErrorMessage = "El código es obligatorio")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El código debe tener 6 dígitos")]
        public string Token { get; set; }
    }
}
