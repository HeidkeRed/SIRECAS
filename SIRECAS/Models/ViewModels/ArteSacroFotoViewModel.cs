using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class ArteSacroFotoViewModel
    {
        public int IdFoto { get; set; }

        public int IdElemento { get; set; }

        [Required]
        public string RutaArchivo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }
    }
}


