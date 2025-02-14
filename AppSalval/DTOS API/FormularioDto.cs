using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSalval.Models_Api
{
    public class FormularioDto
    {
        public int IdFormulario { get; set; }
        public string TituloFormulario { get; set; }
        public string? DescripcionFormulario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Habilitado { get; set; }
        public bool Anonimo { get; set; }

    }
}
