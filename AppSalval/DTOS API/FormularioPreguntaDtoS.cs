using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSalval.DTOS_API
{
    public class FormularioPreguntaDtoS
    {
        public int IdFormularioPregunta { get; set; }
        public int IdFormulario { get; set; }
        public int IdPregunta { get; set; }
        public FormularioPreguntaDtoS(int IdFormulario, int IdPregunta)
        {
            this.IdFormulario = IdFormulario;
            this.IdPregunta = IdPregunta;
        }
    

    }
}
