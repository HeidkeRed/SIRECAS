using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class ElementoArteSacroViewModel
    {
        public int IdElemento { get; set; }

        public int IdIdentificacion { get; set; }

        [Required]
        public string Seccion { get; set; } = string.Empty; // Bienes, Retablo, etc.

        public string? Subtipo { get; set; }

        public string? Nombre { get; set; }

        public string? Descripcion { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public List<ArteSacroFotoViewModel> Fotos { get; set; } = new();
    }
}

