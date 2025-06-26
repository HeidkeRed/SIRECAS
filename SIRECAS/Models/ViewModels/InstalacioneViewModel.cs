using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class InstalacioneViewModel
    {
        public int IdIdentificacion { get; set; }

        [Required]
        public string Tipo { get; set; } = null!;

        public bool Visible { get; set; }

        public bool Oculta { get; set; }

        public string? Observaciones { get; set; }

        //public virtual Identificacion? Identificacion { get; set; }

    }
}

