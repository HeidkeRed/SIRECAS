using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class RegistrarUsuarioViewModel
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rol")]
        public int IdRol { get; set; }

        [Display(Name = "Autorizado")]
        public bool Autorizado { get; set; } = true;
    }
}
