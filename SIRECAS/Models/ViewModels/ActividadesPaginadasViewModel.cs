using System.Collections.Generic;

namespace SIRECAS.Models.ViewModels
{
    public class ActividadesPaginadasViewModel
    {
        public List<ActividadRegistroViewModel> Actividades { get; set; } = new List<ActividadRegistroViewModel>();
        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
    }
}

