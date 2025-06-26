namespace SIRECAS.Models.ViewModels
{
    public class ActividadRegistroViewModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Actividad { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public TimeOnly Hora { get; set; }
    }
}
