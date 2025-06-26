using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class RestablecerContrasenaViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El token es obligatorio.")]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [MinLength(10, ErrorMessage = "La contraseña debe tener al menos 10 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{10,}$",
            ErrorMessage = "La contraseña debe tener al menos una mayúscula, un número y un símbolo.")]
        public string NuevaContrasena { get; set; } = null!;

        [Required(ErrorMessage = "Confirma la nueva contraseña.")]
        [DataType(DataType.Password)]
        [Compare("NuevaContrasena", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasena { get; set; } = null!;
    }
}

