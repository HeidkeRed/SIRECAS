using System.ComponentModel.DataAnnotations;

namespace SIRECAS.Models.ViewModels
{
    public class InstalacioneViewModel
    {
        [Required]
        public int IdIdentificacion { get; set; }

        // Eléctrica
        public bool ElectricaVisible { get; set; }
        public bool ElectricaOculta { get; set; }
        public string? ObservacionesElectrica { get; set; }

        // Sanitaria
        public bool SanitariaVisible { get; set; }
        public bool SanitariaOculta { get; set; }
        public string? ObservacionesSanitaria { get; set; }

        // Hidráulica
        public bool HidraulicaVisible { get; set; }
        public bool HidraulicaOculta { get; set; }
        public string? ObservacionesHidraulica { get; set; }

        // Gas
        public bool GasVisible { get; set; }
        public bool GasOculta { get; set; }
        public string? ObservacionesGas { get; set; }

        // Telefonía
        public bool TelefoniaVisible { get; set; }
        public bool TelefoniaOculta { get; set; }
        public string? ObservacionesTelefonia { get; set; }

        // Otro
        public bool OtroVisible { get; set; }
        public bool OtroOculta { get; set; }
        public string? ObservacionesOtro { get; set; }
    }
}
