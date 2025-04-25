using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = string.Empty;
    }
}
