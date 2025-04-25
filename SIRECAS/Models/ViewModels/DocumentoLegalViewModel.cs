using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class DocumentoLegalViewModel
    {
        public int IdDocumento { get; set; }

        [Required]
        public int IdIdentificacion { get; set; }

        [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
        [StringLength(100)]
        public string NombreDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un archivo.")]
        public IFormFile Archivo { get; set; } = null!;
    }
}

