using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSalval.DTOS_API
{
    public class FormularioPreguntaDto
    {
        public int IdFormularioPregunta { get; set; }
        public int IdFormulario { get; set; }
        public int IdPregunta { get; set; }
    }
}
