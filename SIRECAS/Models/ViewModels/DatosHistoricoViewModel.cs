using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class DatosHistoricoViewModel
    {
        public int IdIdentificacion { get; set; }

        [StringLength(200)]
        public string? Titulo { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(1000)]
        public string Descripcion { get; set; } = null!;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Evento")]
        public DateOnly? FechaEvento { get; set; }
    }
}

