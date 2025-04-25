// ViewModel: RegistroTrabajoViewModel.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class RegistroTrabajoViewModel
    {
        public int IdTrabajo { get; set; }

        public int IdIdentificacion { get; set; }

        [Display(Name = "Tipo de Registro")]
        public string? TipoRegistro { get; set; }

        [Display(Name = "Descripción del Trabajo")]
        public string? Descripcion { get; set; }

        [Display(Name = "Empresa Responsable")]
        public string? EmpresaResponsable { get; set; }

        [Display(Name = "Fecha del Trabajo")]
        [DataType(DataType.Date)]
        public DateOnly? FechaTrabajo { get; set; }

        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }
    }
}


