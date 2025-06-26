using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class DanoFotoViewModel
    {
        public int IdFoto { get; set; }

        public int IdDano { get; set; }

        [Required]
        public string RutaArchivo { get; set; } = string.Empty;

        public string? Observacion { get; set; }

    }
}
