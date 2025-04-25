using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class DanoObservableViewModel
    {
        public int IdDano { get; set; }

        public int IdIdentificacion { get; set; }

        [Required]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        public string Zona { get; set; } = string.Empty;

        [Required]
        public string Estado { get; set; } = string.Empty;

        public List<DanoFotoViewModel> Fotos { get; set; } = new();
    }
}

